using PPCalculator.Simulate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace pp_server
{
    public class PPHelper
    {
        private readonly static WebClient client = new WebClient();

        public static string GetPP(
            string osuFilePath, 
            int bid, 
            int mode, 
            string mods, 
            double acc, 
            int miss, 
            int combo, 
            int good, 
            int meh,
            int score,
            double percentCombo)
        {
            //初始化计算对象数据
            SimulateCommand command = null;
            switch (mode)
            {
                case 0:
                    command = new OsuSimulateCommand();
                    break;
                case 1:
                    command = new TaikoSimulateCommand();
                    break;
                case 3:
                    command = new ManiaSimulateCommand();
                    break;
            }

            //校验传入参数
            if (command == null ||
                bid <= 0 ||
                miss < 0 ||
                combo < 0 ||
                good < 0 ||
                meh < 0 ||
                (acc < 0 && acc > 100) ||
                (score < 0 && score > 1000000 && mode == 3) ||
                (score < 0 && mode != 3) ||
                (percentCombo < 0 && percentCombo > 100))
            { 
                return "[]";
            }

            

            try 
            {
                var path = osuFilePath + Path.DirectorySeparatorChar + bid + ".osu";

                FileInfo info = new FileInfo(path);
                if (!info.Exists)
                {
                    string content = client.DownloadString("http://osu.ppy.sh/osu/" + bid);
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        File.WriteAllText(path, content);
                    }
                }

                //设置osu文件路径
                command.Beatmap = path;


                //设置acc
                command.Accuracy = (acc > 0) ? acc : 100;


                //设置miss
                if(miss > 0) command.Misses = miss;
     
                
                //设置combo
                if(percentCombo > 0)
                {
                    command.PercentCombo = percentCombo;
                }
                else if(combo > 0)
                {
                    command.Combo = combo;
                }

                //设置100数量
                if(good > 0) command.Goods = good;

                //设置50数量
                if (meh > 0) command.Mehs = meh;

                //设置游玩模式
                command.Mods = GetMods(mods);

                //Mania的分数处理
                if(command is ManiaSimulateCommand)
                {
                    if(score > 0)
                    {
                        command.Score = score;
                    }
                    else
                    {
                        command.Score = 1000000;
                    }

                    foreach(var item in command.Mods)
                    {
                        if (item.Equals("EZ") || item.Equals("HT") || item.Equals("NF"))
                        {
                            command.Score = (int)(command.Score * 0.5);
                        }
                    }
                }

                //开始计算
                command.Execute();
            }
            catch
            {
                
            }


            return command.ToJson();
        }

        private static string[] GetMods(string mods)
        {
            List<string> list = new List<string>();

            if(mods != null)
            {
                for (int i = 0; i < mods.Length; i += 2)
                {
                    int end = i + 2;
                    if (end <= mods.Length)
                    {
                        list.Add(mods.Substring(i, 2));
                    }
                }
            }

            return list.ToArray();
        }
    }
}
