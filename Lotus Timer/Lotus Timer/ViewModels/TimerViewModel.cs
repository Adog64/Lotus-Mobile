using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Lotus_Timer.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Lotus_Timer.ViewModels
{
    public class TimerViewModel : BaseViewModel
    {
        public ICommand TimerButtonCommand { get; }
        double _time;
        Stopwatch _timer;
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
            _timer = new Stopwatch();
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
                        /*ClockFace = "dnf";
                        _timerState = TimerState.STOPPED;*/
                        return false;
                    });
                    break;
                case TimerState.INSPECTION:
                    _timer.Start();
                    _timerState = TimerState.TIMING;
                    _time = 0;                          // clear time left from inspection
                    ClockFace = _time.ToString();       // update timer display
                    // start timer
                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                        if (_timerState == TimerState.TIMING)
                        {
                            _time++;
                            if (TimeSpan.FromSeconds(_time).Minutes > 0)
                                ClockFace = TimeSpan.FromSeconds(_time).ToString(@"%m\:ss");
                            else
                                ClockFace = TimeSpan.FromSeconds(_time).ToString(@"%s");
                            return true;
                        }
                        return false;
                    });
                    break;
                case TimerState.TIMING:
                    _timer.Stop();
                    _timerState = TimerState.STOPPED;
                    _time = Math.Round(_timer.Elapsed.TotalSeconds, 2);
                    if (TimeSpan.FromSeconds(_time).Minutes > 0)
                        ClockFace = TimeSpan.FromSeconds(_time).ToString(@"%m\:ss\.ff");
                    else
                        ClockFace = TimeSpan.FromSeconds(_time).ToString(@"%s\.ff");
                    Scramble = _scrambler.generateScramble();
                    break;
                case TimerState.STOPPED:
                    ClockFace = "Ready";
                    _timerState = TimerState.READY;
                    _timer.Reset();
                    break;
            }
        }

        // private class for quickly generating scrambles
        private class Scrambler
        {
            string cubeType;
            private readonly string[] R_MOVES = { "R", "R'", "R2" },
                                      U_MOVES = { "U", "U'", "U2" },
                                      F_MOVES = { "F", "F'", "F2" },
                                      L_MOVES = { "L", "L'", "L2" },
                                      D_MOVES = { "D", "D'", "D2" },
                                      B_MOVES = { "B", "B'", "B2" },
                                      Rw_MOVES = { "Rw", "Rw'", "Rw2" },
                                      Uw_MOVES = { "Uw", "Uw'", "Uw2" },
                                      Fw_MOVES = { "Fw", "Fw'", "Fw2" },
                                      Lw_MOVES = { "Lw", "Lw'", "Lw2" },
                                      Dw_MOVES = { "Dw", "Dw'", "Dw2" },
                                      Bw_MOVES = { "Bw", "Bw'", "Bw2" },
                                      TRw_MOVES = { "3Rw", "3Rw'", "3Rw2" },
                                      TUw_MOVES = { "3Uw", "3Uw'", "3Uw2" },
                                      TFw_MOVES = { "3Fw", "3Fw'", "3Fw2" },
                                      TLw_MOVES = { "3Lw", "3Lw'", "3Lw2" },
                                      TDw_MOVES = { "3Dw", "3Dw'", "3Dw2" },
                                      TBw_MOVES = { "3Bw", "3Bw'", "3Bw2" };

            private List<string[]> moveSet;
            private int scrambleSize;
            public Scrambler(string cubeType)
            {
                this.cubeType = cubeType;
                moveSet = new List<string[]>();
                switch (cubeType)
                {
                    case "777":
                        moveSet.Add(TLw_MOVES);
                        moveSet.Add(TBw_MOVES);
                        moveSet.Add(TDw_MOVES);
                        goto case "666";
                    case "666":
                        moveSet.Add(TUw_MOVES);
                        moveSet.Add(TFw_MOVES);
                        moveSet.Add(TRw_MOVES);
                        goto case "555";
                    case "555":
                        moveSet.Add(Lw_MOVES);
                        moveSet.Add(Bw_MOVES);
                        moveSet.Add(Dw_MOVES);
                        goto case "444";
                    case "444":
                        moveSet.Add(Uw_MOVES);
                        moveSet.Add(Fw_MOVES);
                        moveSet.Add(Rw_MOVES);
                        goto case "333";
                    case "333":
                        moveSet.Add(L_MOVES);
                        moveSet.Add(B_MOVES);
                        moveSet.Add(D_MOVES);
                        goto case "222";
                    case "222":
                        moveSet.Add(U_MOVES);
                        moveSet.Add(F_MOVES);
                        moveSet.Add(R_MOVES);
                        break;
                }
                switch (cubeType)
                {
                    case "777":
                        scrambleSize = 100;
                        break;
                    case "666":
                        scrambleSize = 80;
                        break;
                    case "555":
                        scrambleSize = 60;
                        break;
                    case "444":
                        scrambleSize = 45;
                        break;
                    case "333":
                        scrambleSize = 20;
                        break;
                    case "222":
                        scrambleSize = 10;
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

        // private class for managing time data
        private class SessionManager
        {
            
            public SessionManager()
            {
                
            }

            public void CreateSession(string cubeType)
            {

            }
        }
    }
}
