using PPCalculator.Simulate;
using System;

namespace PPCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
             * 改造步骤
             * 
             * 1. 复制ProcessorCommand.cs，以及Simulate目录下的文件到新项目中，并删除无用文件
             * 
             * 2. 在依赖项引入项目PerformanceCalculator， 并将Simulate目录下类的命名空间，更改为当前项目下的命名空间
             * 
             * 3. 给SimulateCommand及其子类的字段添加set访问属性
             * 
             * 4. 将ProcessorCommand类中的Console字段删除
             * 
             * 5. 添加一个字段到SimulateCommand类中 public Dictionary<string, string> Result = new Dictionary<string, string>();
             * 
             * 6. 修改SimulateCommand里边的WriteAttribute方法,将每次调用的键值对赋值给Result对象
             * 
             * 7. 修改OsuSimulateCommand和TaikoSimulateCommand里边的WritePlayInfo方法，将Accuracy改为Acc防止字段冲突
             * 
             * 8. 添加ToJson方法到SimulateCommand类中 public string ToJson() => JsonConvert.SerializeObject(Result);
             * 
             * 收工
             */

            SimulateCommand command = new OsuSimulateCommand();
            command.Beatmap = "c:\\osu\\osu.osu";
            command.Accuracy = 75.3;
            command.Combo = 2000;
            command.Misses = 20;
            command.Execute();
            Console.WriteLine(command.ToJson());
        }
    }
}
