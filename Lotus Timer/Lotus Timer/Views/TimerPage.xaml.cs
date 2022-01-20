using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LotusTimer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using LotusTimer.Models;

namespace LotusTimer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimerPage : ContentPage
    {
        public TimerPage()
        {
            BindingContext = new TimerViewModel();
            InitializeComponent();
        }
        public async void ChangeSession(object sender, EventArgs e)
        {
            Debug.WriteLine("Opening sessions list...");
            await Navigation.PushModalAsync(new SessionControlPage());
        }
    }
}