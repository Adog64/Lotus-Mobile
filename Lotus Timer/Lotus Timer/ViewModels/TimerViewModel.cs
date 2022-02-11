using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using LotusTimer.Models;
using System.Diagnostics;

namespace LotusTimer.ViewModels
{
    public class TimerViewModel : CubeTimingViewModel
    {
        public ICommand TimerButtonCommand { get; }
        public ICommand DnfCommand { get; }
        public ICommand SessionChangeCommand { get; }

        public ICommand Plus2Command { get; }
        double _time;
        Stopwatch _timer;
        const double TICK = 0.01;
        float _progress;
        bool _showingTimeModifiers;
        bool _showingScramble;
        sbyte _penalty;
        string _scramble;
        string _clockFace;
        TimerState _timerState;

        public bool ShowingScramble
        {
            get { return _showingScramble; }
            private set { SetProperty<bool>(ref _showingScramble, value); }
        }
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
        public string Scramble
        {
            get { return _scramble; }
            set { SetProperty(ref _scramble, String.Copy(value)); }
        }

        public bool ShowingTimeModifiers
        {
            get { return _showingTimeModifiers; }
            set { SetProperty(ref _showingTimeModifiers, value); }
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
            _time = 0;
            _penalty = 0;
            _showingTimeModifiers = false;
            _timer = new Stopwatch();
            _progress = 1;
            _timerState = TimerState.READY;
            ShowingScramble = true;
            ClockFace = "Ready";
            SessionIndex = 0;
            Scrambler.generateScramble("sqn");
            Scramble = Scrambler.generateScramble(SessionManager.CurrentSession.CubeType);
            TimerButtonCommand = new Command(() => Next());
            DnfCommand = new Command(() => Dnf());
            Plus2Command = new Command(() => Plus2());
            SessionChangeCommand = new Command((item) =>
            {
                SessionIndex = SessionNames.IndexOf(item.ToString());
                SessionManager.SetSession(SessionIndex);
                Scramble = Scrambler.generateScramble(SessionManager.CurrentSession.CubeType);
                UpdatePageStats();
            });
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
                    ShowingTimeModifiers = true;
                    PublishSolve();
                    break;
                case TimerState.STOPPED:
                    ClockFace = "Ready";
                    ShowingScramble = true;
                    ShowingTimeModifiers = false;
                    Progress = 1;
                    Scramble = Scrambler.generateScramble(SessionManager.CurrentSession.CubeType);
                    _timerState = TimerState.READY;
                    _timer.Reset();
                    break;
            }
        }

        public void DoInspection()
        {
            _timerState = TimerState.INSPECTION;
            Progress = 1;
            _timer.Restart();
            _time = 15;                     // length of inpection
            ShowingScramble = false;        // stop showing scramble
            ClockFace = _time.ToString();   // update display on timer
                                            // start inspection timer
            Device.StartTimer(TimeSpan.FromSeconds(TICK), () =>
            {
                if (_timerState == TimerState.INSPECTION && _time > 0)
                {
                    _time = 15 - _timer.Elapsed.TotalSeconds;
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
            _timer.Restart();
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
            _penalty = _penalty != -1 ? (sbyte)-1 : (sbyte)0;
            SessionManager.LatestSolve.Penalty = _penalty;
            ClockFace = SessionManager.LatestSolve.ToString();
            UpdatePageStats();
        }
        public void Plus2()
        {
            _penalty = _penalty != 2 ? (sbyte)2 : (sbyte)0;
            SessionManager.LatestSolve.Penalty = _penalty;
            ClockFace = SessionManager.LatestSolve.ToString();
            UpdatePageStats();
        }


        // update statistics that can be seen on-screen
        public void PublishSolve()
        {
            _time = Math.Round(_timer.Elapsed.TotalSeconds, 2);
            Solve currentSolve = new Solve();
            currentSolve.Scramble = _scramble;
            currentSolve.Time = _time;
            currentSolve.Penalty = _penalty;
            currentSolve.Timestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            SessionManager.Publish(currentSolve);
            ClockFace = SessionManager.LatestSolve.ToString();
            UpdatePageStats();
        }
    }
}