using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus_Timer.Models
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
            if (TimeSpan.FromSeconds(Time + Penalty).Hours > 0)
                return TimeSpan.FromSeconds(Time + Penalty).ToString(@"%h\:mm\:ss\.ff");
            if (TimeSpan.FromSeconds(Time + Penalty).Minutes > 0)
                return TimeSpan.FromSeconds(Time + Penalty).ToString(@"%m\:ss\.ff");
            return TimeSpan.FromSeconds(Time + Penalty).ToString(@"%s\.ff");
        }

    }
}
