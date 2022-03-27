using System.ComponentModel;
using Xamarin.Forms;
using ImageMatch.ViewModels;

namespace ImageMatch.Views
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
