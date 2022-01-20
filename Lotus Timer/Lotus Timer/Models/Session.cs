using System;
using System.Collections.Generic;
using System.Text;

namespace LotusTimer.Models
{
    public class Session
    {
        public double Mo3 { get; set; }
        public double Ao5 { get; set; }
        public double Ao12 { get; set; }
        public double Ao100 { get; set; }
        public double Ao1000 { get; set; }
        public double Worst { get; set; }
        public double Best { get; set; }
        public double Average { get; set; }
        public List<Solve> Solves { get; set; }
        public string CubeType { get; set; }
        public string Name { get; set; }
    }
}
