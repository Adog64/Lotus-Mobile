using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Lotus_Timer.ViewModels;
using Lotus_Timer.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Lotus_Timer.Models;

namespace Lotus_Timer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatsPage : ContentPage
    {
        public StatsPage()
        {
            BindingContext = new StatsViewModel();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ((StatsViewModel)BindingContext).Refresh();
        }

        private async void OpenSolvePage(object sender, EventArgs e)
        {
            if (e.GetType() == typeof(SelectedItemChangedEventArgs))
            {
                int item = ((SelectedItemChangedEventArgs)e).SelectedItemIndex;
                await Navigation.PushModalAsync(new SolvePage(item));
            }
        }
    }
}