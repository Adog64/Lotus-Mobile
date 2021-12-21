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
        public Timer timer;
        public string TimerText { get; set; }
        public string Scramble { get; set; }

        public TimerViewModel()
        {
            Title = "Timer";
            timer = new Timer();
            TimerText = timer.clockFace;
            Scramble = timer.scramble;
            TimerButtonCommand = new Command(() => Next());
        }

        public void Next()
        {
            timer.GenerateScramble();
            Scramble = timer.scramble;
            Debug.WriteLine(Scramble);
        }
    }
}
