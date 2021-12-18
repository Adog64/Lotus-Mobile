using Lotus_Timer.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Lotus_Timer.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}