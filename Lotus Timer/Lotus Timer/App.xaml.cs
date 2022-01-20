using LotusTimer.Services;
using LotusTimer.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LotusTimer
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new TabsPage();
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
