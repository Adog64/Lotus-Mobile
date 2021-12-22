using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Lotus_Timer.Models;
using System.Diagnostics;

namespace Lotus_Timer.ViewModels
{
    public class TimerViewModel : BaseViewModel
    {
        public ICommand TimerButtonCommand { get; }
        float _time;
        string _scramble;
        string _clockFace;
        Scrambler _scrambler;
        TimerState _timerState;
        public string ClockFace 
        {
            get { return _clockFace; }
            set { SetProperty(ref _clockFace, String.Copy(value)); }
        }
        public string Scramble { 
            get { return _scramble; }
            set { SetProperty(ref _scramble, String.Copy(value)); }
        }

        public enum TimerState
        {
            READY,
            INSPECTION,
            TIMING,
            STOPPED
        }

        public TimerViewModel()
        {
            Title = "Timer";
            _scrambler = new Scrambler("333");
            _time = 0;
            _timerState = TimerState.READY;
            ClockFace = "Ready";
            Scramble = _scrambler.generateScramble();
            TimerButtonCommand = new Command(() => Next());
        }

        // incrament timer state and execute the next step of the timer
        public void Next()
        {
            switch (_timerState)
            {
                case TimerState.READY:
                    _timerState = TimerState.INSPECTION;
                    _time = 15;                     // length of inpection
                    Scramble = "";                  // make scramble invisible
                    ClockFace = _time.ToString();   // update display on timer
                    // start inspection timer
                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                        if (_timerState == TimerState.INSPECTION && _time > 0)
                        {
                            _time--;
                            ClockFace = _time.ToString();
                            return true;
                        }
                        // automatically start timing if inspection runs out
                        _timerState = TimerState.TIMING;
                        return false;
                    });
                    break;
                case TimerState.INSPECTION:
                    _timerState = TimerState.TIMING;
                    _time = 0;                          // clear time left from inspection
                    ClockFace = _time.ToString();       // update timer display

                    // start timer
                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                        if (_timerState == TimerState.TIMING)
                        {
                            _time++;
                            ClockFace = _time.ToString();
                            return true;
                        }
                        return false;
                    });
                    break;
                case TimerState.TIMING:
                    _timerState = TimerState.STOPPED;
                    Scramble = _scrambler.generateScramble();
                    break;
                case TimerState.STOPPED:
                    ClockFace = "Ready";
                    _timerState = TimerState.READY;
                    break;
            }
        }

        private class Scrambler
        {
            string cubeType;
            private readonly string[] R_MOVES = { "R", "R'", "R2" },
                                      U_MOVES = { "U", "U'", "U2" },
                                      F_MOVES = { "F", "F'", "F2" },
                                      L_MOVES = { "L", "L'", "L2" },
                                      D_MOVES = { "D", "D'", "D2" },
                                      B_MOVES = { "B", "B'", "B2" };

            private List<string[]> moveSet;
            private int scrambleSize;
            public Scrambler(string cubeType)
            {
                moveSet = new List<string[]>();
                switch (cubeType)
                {
                    case "222":
                        scrambleSize = 8;
                        moveSet.Add(U_MOVES);
                        moveSet.Add(F_MOVES);
                        moveSet.Add(R_MOVES);
                        break;
                    case "333":
                        scrambleSize = 20;
                        moveSet.Add(U_MOVES);
                        moveSet.Add(F_MOVES);
                        moveSet.Add(R_MOVES);
                        moveSet.Add(L_MOVES);
                        moveSet.Add(B_MOVES);
                        moveSet.Add(D_MOVES);
                        break;
                }
            }

            public string generateScramble()
            {
                Random random = new Random();

                List<string[]> scramblePrototype = new List<string[]>();    // scrambled faces (R, U, L) without regards to rotation direction or amount
                string scramble = "";                                       // the finished scramble to be returned

                for (int i = 0; i < scrambleSize; i++)
                {
                    scramblePrototype.Add(moveSet[random.Next(moveSet.Count)]);

                    // basically dont do 2 of the same move in a row (Ex. U followed by U2)
                    while (i > 0 && scramblePrototype[i - 1] == scramblePrototype[i])
                        scramblePrototype[i] = moveSet[random.Next(moveSet.Count)];
                }

                foreach (string[] moveType in scramblePrototype)
                {
                    scramble += moveType[random.Next(3)] + " ";
                }

                scramble = scramble.Substring(0, scramble.Length - 1);      // remove the trailing space

                return scramble;
            }
        }
    }
}
