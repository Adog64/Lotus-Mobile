using System;
using System.Collections.Generic;
using System.Text;
using Lotus_Timer.Models;

namespace Lotus_Timer.ViewModels
{
    public class StatsViewModel : BaseViewModel
    {
        protected string _best, _worst, _ao5, _ao12, _ao100, _ao1000;

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

        public StatsViewModel()
        {
            SessionManager.Load();
            UpdatePageStats();
        }

        public List<Solve> Solves
        {
            get { return SessionManager.CurrentSession.Solves; }
        }

        public void UpdatePageStats()
        {
            SessionManager.UpdateSessionStats();
            Best = "Best: " + FormatTime(SessionManager.CurrentSession.Best);
            Worst = "Worst: " + FormatTime(SessionManager.CurrentSession.Worst);
            Ao5 = "Average of 5: " + FormatTime(SessionManager.CurrentSession.Ao5);
            Ao12 = "Average of 12: " + FormatTime(SessionManager.CurrentSession.Ao12);
            Ao100 = "Average of 100: " + FormatTime(SessionManager.CurrentSession.Ao100);
            Ao1000 = "Average of 1000: " + FormatTime(SessionManager.CurrentSession.Ao1000);
        }

        // format seconds into a standard time format
        public string FormatTime(double time)
        {
            if (time == -1)
                return "dnf";
            if (time == 0)
                return "-.-";
            if (TimeSpan.FromSeconds(time).Hours > 0)
                return TimeSpan.FromSeconds(time).ToString(@"%h\:mm\:ss\.ff");
            if (TimeSpan.FromSeconds(time).Minutes > 0)
                return TimeSpan.FromSeconds(time).ToString(@"%m\:ss\.ff");
            return TimeSpan.FromSeconds(time).ToString(@"%s\.ff");
        }
    }
}
