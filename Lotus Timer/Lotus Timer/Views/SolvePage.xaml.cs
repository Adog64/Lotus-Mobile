using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LotusTimer.Models;
using LotusTimer.ViewModels;

namespace LotusTimer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SolvePage : ContentPage
    {
        public SolvePage(int solveIndex)
        {
            BindingContext = new SolveViewModel(solveIndex);
            InitializeComponent();
        }

        private async void Return(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}