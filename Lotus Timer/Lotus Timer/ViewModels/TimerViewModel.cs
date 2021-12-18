using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Lotus_Timer.Models;

namespace Lotus_Timer.ViewModels
{
    public class TimerViewModel : BaseViewModel
    {
        public ICommand TimerButtonCommand { get; }
        protected Timer timer;
        private string scramble;
        public string TimerText
        {
            get { return timer.ClockFace(); }
        }
        public string Scramble
        {
            get { return scramble; }
        }
        public TimerViewModel()
        {
            Title = "Timer";
            timer = new Timer();
            scramble = timer.scramble;
            TimerButtonCommand = new Command(() => reloadScramble());
        }

        public void reloadScramble()
        {
            timer.ClockFace();
            timer.GenerateScramble();
            scramble = timer.scramble;
        }
    }
}
