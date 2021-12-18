using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus_Timer.Models
{
    public class Timer
    {
        public string cubeType, scramble;
        public double time;
        private TimerState state;
        private Scrambler scrambler;
        public enum TimerState
        {
            READY,
            INSPECTION,
            TIMING,
            STOPPED
        }

        public Timer() : this("333")
        {
        }

        public Timer(string cubeType)
        {
            this.cubeType = cubeType;
            scrambler = new Scrambler(cubeType);
            GenerateScramble();
        }

        public string ClockFace()
        {
            if (state == TimerState.READY)
                return "Ready?";
            else
                return Convert.ToString(time);

        }

        public void GenerateScramble()
        {
            scramble = scrambler.generateScramble();
        }

        public void Next()
        {
            if (state == TimerState.READY)
            {
                state = TimerState.INSPECTION;
                time = 0;
            }
            else if (state == TimerState.INSPECTION)
            {
                state = TimerState.TIMING;
                time = 15;
            }
            else if (state == TimerState.TIMING)
            {
                state = TimerState.STOPPED;
                time = 0;
            }
            else if (state == TimerState.STOPPED)
            {
                state = TimerState.READY;
            }
        }
    }
}
