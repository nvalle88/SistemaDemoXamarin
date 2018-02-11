using AppDemo.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppDemo.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsultarAutoPage : ContentPage
    {
        public ConsultarAutoPage()
        {
            InitializeComponent();
            var main = (MainViewModel)BindingContext;
            base.Appearing += (object sender, EventArgs e) =>
            {
                main.LoadVehiculosMultados();
            };
        }

        public void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", " delete context action", "OK");
        }
    }
}