using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus_Timer.Models
{
    public class Timer
    {
        public string cubeType, scramble;
        public double time;
        public string clockFace;
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
            clockFace = "Ready";
            this.cubeType = cubeType;
            scrambler = new Scrambler(cubeType);
            GenerateScramble();
        }

        public void GenerateScramble()
        {
            scramble = scrambler.generateScramble();
        }

        public void Next()
        {
            switch (state)
            {
                case TimerState.READY:
                    state = TimerState.INSPECTION;
                    time = 15;
                    clockFace = time.ToString();
                    break;
                case TimerState.INSPECTION:
                    state = TimerState.TIMING;
                    time = 0;
                    clockFace = time.ToString();
                    break;
                case TimerState.TIMING:
                    state = TimerState.STOPPED;
                    clockFace = time.ToString();
                    break;
                case TimerState.STOPPED:
                    state = TimerState.INSPECTION;
                    clockFace = "Ready";
                    break;
            }
        }
    }
}
