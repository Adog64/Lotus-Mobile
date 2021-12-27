using System;
using System.Collections.Generic;
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
        public ICommand DnfCommand { get; }
        public ICommand Plus2Command { get; }
        double _time;
        Stopwatch _timer;
        const double TICK = 0.01;
        float _progress;
        bool _timeModifiers;
        sbyte _penalty;
        string _scramble;
        string _clockFace;
        string _best, _worst, _ao5, _ao12, _ao100, _ao1000;
        Scrambler _scrambler;
        TimerState _timerState;
        SessionManager _sessionManager;
        public string ClockFace
        {
            get { return _clockFace; }
            set { SetProperty(ref _clockFace, String.Copy(value)); }
        }
        public float Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }
        public string Scramble { 
            get { return _scramble; }
            set { SetProperty(ref _scramble, String.Copy(value)); }
        }   
        public string Best
        {
            get { return _best; }
            set { SetProperty(ref _best, String.Copy(value)); }
        }
        public string Worst
        {
            get { return _worst; }
            set { SetProperty(ref _worst, String.Copy(value)); }
        }
        public string Ao5
        {
            get { return _ao5; }
            set { SetProperty(ref _ao5, String.Copy(value)); }
        }
        public string Ao12
        {
            get { return _ao12; }
            set { SetProperty(ref _ao12, String.Copy(value)); }
        }
        public string Ao100
        {
            get { return _ao100; }
            set { SetProperty(ref _ao100, String.Copy(value)); }
        }
        public string Ao1000
        {
            get { return _ao1000; }
            set { SetProperty(ref _ao1000, String.Copy(value)); }
        }
        public bool TimeModifiers
        {
            get { return _timeModifiers; }
            set { SetProperty(ref _timeModifiers, value); }
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
            _penalty = 0;
            _timeModifiers = false;
            _timer = new Stopwatch();
            _sessionManager = new SessionManager();
            _progress = 1;
            _timerState = TimerState.READY;
            UpdateUserStats();
            ClockFace = "Ready";
            Scramble = _scrambler.generateScramble();
            TimerButtonCommand = new Command(() => Next());
            DnfCommand = new Command(() => Dnf());
            Plus2Command = new Command(() => Plus2());
        }

        // incrament timer state and execute the next step of the timer
        public void Next()
        {
            switch (_timerState)
            {
                case TimerState.READY:
                    DoInspection();
                    break;
                case TimerState.INSPECTION:
                    DoTiming();
                    break;
                case TimerState.TIMING:
                    _timer.Stop();
                    _timerState = TimerState.STOPPED;
                    TimeModifiers = true;
                    UpdateUserStats();
                    break;
                case TimerState.STOPPED:
                    ClockFace = "Ready";
                    TimeModifiers = false;
                    Progress = 1;
                    Scramble = _scrambler.generateScramble();
                    _timerState = TimerState.READY;
                    _timer.Reset();
                    break;
            }
        }

        public void DoInspection()
        {
            _timerState = TimerState.INSPECTION;
            Progress = 1;
            _time = 15;                     // length of inpection
            Scramble = "";                  // make scramble invisible
            ClockFace = _time.ToString();   // update display on timer
                                            // start inspection timer
            Device.StartTimer(TimeSpan.FromSeconds(TICK), () =>
            {
                if (_timerState == TimerState.INSPECTION && _time > 0)
                {
                    _time -= TICK;
                    Progress = (float)(_time / 15);
                    ClockFace = ((int)_time).ToString();
                    return true;
                }
                return false;
            });
        }

        public void DoTiming()
        {
            _timerState = TimerState.TIMING;
            _time = 0;                          // clear time
            _penalty = 0;
            _timer.Start();
            ClockFace = _time.ToString();       // update timer display
            Progress = 0;
            // start timer
            Device.StartTimer(TimeSpan.FromSeconds(TICK), () =>
            {
                if (_timerState == TimerState.TIMING)
                {
                    _time += TICK;
                    Progress = (float)((_timer.Elapsed.TotalSeconds % 60) / 60);
                    ClockFace = ((int)_timer.Elapsed.TotalSeconds).ToString();
                    return true;
                }
                return false;
            });
        }

        public void Dnf()
        {
            _penalty = -1;
            UpdateUserStats();
        }
        public void Plus2()
        {
            if (_penalty != -1)
                _penalty = 2;
            UpdateUserStats();
        }
        
        // update statistics that can be seen on-screen
        public void UpdateUserStats()
        {
            _time = Math.Round(_timer.Elapsed.TotalSeconds, 2);
            ClockFace = _penalty == 0 ? FormatTime(_time) : (_penalty == -1) ? "dnf" : (FormatTime(_time) + "+2");
            Solve currentSolve = new Solve();
            currentSolve.Scramble = Scramble;
            currentSolve.Time = _time;
            currentSolve.Penalty = _penalty;
            currentSolve.Timestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            _sessionManager.Publish(currentSolve);
            Best = "Best: " + FormatTime(_sessionManager.CurrentSession.Best);
            Worst = "Worst: " + FormatTime(_sessionManager.CurrentSession.Worst);
            Ao5 = "Average of 5: " + FormatTime(_sessionManager.CurrentSession.Ao5);
            Ao12 = "Average of 12: " + FormatTime(_sessionManager.CurrentSession.Ao12);
            Ao100 = "Average of 100: " + FormatTime(_sessionManager.CurrentSession.Ao100);
            Ao1000 = "Average of 1000: " + FormatTime(_sessionManager.CurrentSession.Ao1000);
        }

        // format seconds into a standard time format
        public string FormatTime(double seconds)
        {
            if (seconds <= 0)
                return "-.-";
            if (TimeSpan.FromSeconds(seconds).Hours > 0)
                return TimeSpan.FromSeconds(seconds).ToString(@"%h\:mm\:ss\.ff");
            if (TimeSpan.FromSeconds(seconds).Minutes > 0)
                return TimeSpan.FromSeconds(seconds).ToString(@"%m\:ss\.ff");
            return TimeSpan.FromSeconds(seconds).ToString(@"%s\.ff");
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
    }
}