using Lotus_Timer.Services;
using Lotus_Timer.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lotus_Timer
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new TimerPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
