using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lotus_Timer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            BindingContext = new StatsViewModel();
        }
    }
}