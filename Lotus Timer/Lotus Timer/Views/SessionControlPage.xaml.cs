using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LotusTimer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SessionControlPage : ContentPage
    {
        public SessionControlPage()
        {
            InitializeComponent();
        }

        private async void Return(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}