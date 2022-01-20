using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LotusTimer.ViewModels
{
    public class AboutViewModel
    {
        public string Title { get; set; }
        public Command DiscordCommand { get; set; }

        public AboutViewModel()
        {
            Title = "About";
            DiscordCommand = new Command(async() => await Browser.OpenAsync("https://discord.gg/TVdnTnJg6n"));
        }


    }
}
