using System;
using System.Collections.Generic;
using System.Text;
using Lotus_Timer.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Lotus_Timer.ViewModels
{
    public class SolveViewModel : BaseViewModel
    {
        private Solve _solve;
        int _solveIndex;
        string _officialTime;
        public string Scramble
        {
            get { return _solve.Scramble; }
            private set { SetProperty(ref _solve.Scramble, String.Copy(value)); }
        }
        public string OfficialTime
        {
            get { return _officialTime; }
            set { SetProperty(ref _officialTime, String.Copy(value)); }
        }

        public Command DeleteCommand { get; }
        public Command DnfCommand { get; }
        public Command Plus2Command { get; }
        public SolveViewModel(int solveIndex)
        {
            _solve = SessionManager.CurrentSession.Solves[solveIndex];
            _solveIndex = solveIndex;
            _officialTime = _solve.ToString();

            DeleteCommand = new Command(() => SessionManager.DeleteFromCurrent(_solveIndex));
            DnfCommand = new Command(() => Dnf());
            Plus2Command = new Command(() => Plus2());
        }

        public void Dnf()
        {
            _solve.Penalty = _solve.Penalty != -1 ? (sbyte)-1 : (sbyte)0;
            OfficialTime = _solve.ToString();
            SessionManager.UpdateSessionStats();
        }
        public void Plus2()
        {
            _solve.Penalty = _solve.Penalty != 2 ? (sbyte)2 : (sbyte)0;
            OfficialTime = _solve.ToString();
            SessionManager.UpdateSessionStats();
        }
    }
}
