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
        Timer timer;
        string _scramble;
        string _clockFace;
        public string ClockFace 
        {
            get { return _clockFace; }
            set { SetProperty(ref _clockFace, String.Copy(value)); }
        }

        public string Scramble { 
            get { return _scramble; }
            set { SetProperty(ref _scramble, String.Copy(value)); }
        }

        public TimerViewModel()
        {
            Title = "Timer";
            timer = new Timer();
            _clockFace = "Ready";
            _scramble = timer.scramble;
            TimerButtonCommand = new Command(() => Next());
        }

        public void Next()
        {
            timer.GenerateScramble();
            Scramble = timer.scramble;
            ClockFace = 15.ToString();
        }
    }
}
