using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus_Timer.Models
{
    public class Session
    {
        public float Mo3 { get; set; }
        public float Ao5 { get; set; }
        public float Ao12 { get; set; }
        public float Ao100 { get; set; }
        public float Ao1000 { get; set; }
        public float Worst { get; set; }
        public float Best { get; set; }
        public float Average { get; set; }
        public IList<Solve> Solves { get; set; }
        public string CubeType { get; set; }
    }
}
