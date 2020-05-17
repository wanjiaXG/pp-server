using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pp_server
{
    public class PPModel
    {
        public int BeatmapId { set; get; }
        public double Accuracy { set; get; }
        public int Combo { set; get; }
        public int Great { set; get; }
        public int Good { set; get; }
        public int Meh { set; get; }
        public int Miss { set; get; }
        public string[] Mods { set; get; }
        public double Aim { set; get; }
        public double Speed { set; get; }
        public double Acc { set; get; }
        public double OD { set; get; }
        public double AR { set; get; }
        public int MaxCombo { set; get; }
        public double PP { set; get; }
    }
}
