using System;
using System.Collections.Generic;
using System.Text;

namespace LotusTimer.Models
{
    public class Solve
    {
        public double Time;
        public double Timestamp;
        public string Scramble;
        public int Penalty;

        // format seconds into a standard time format
        public override string ToString()
        {
            if (Penalty == -1)
                return "DNF";
            if (Time == 0)
                return "-.-";
            string s;
            if (TimeSpan.FromSeconds(Time + Penalty).Hours > 0)
                s = TimeSpan.FromSeconds(Time + Penalty).ToString(@"%h\:mm\:ss\.ff");
            else if (TimeSpan.FromSeconds(Time + Penalty).Minutes > 0)
                s = TimeSpan.FromSeconds(Time + Penalty).ToString(@"%m\:ss\.ff");
            else
                s = TimeSpan.FromSeconds(Time + Penalty).ToString(@"%s\.ff");
            return (Penalty == 0) ? s : s + "(+2)";
        }

    }
}
