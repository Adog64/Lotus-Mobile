using System;
using LotusTimer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LotusTimer.Views
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