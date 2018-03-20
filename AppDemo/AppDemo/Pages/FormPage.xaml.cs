using AppDemo.Models;
using AppDemo.ViewModels;
using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppDemo.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormPage : ContentPage
    {
        InformeViewModel viewmodel;
        Visita _visita;

        public FormPage(Visita visita)
        {
            InitializeComponent();
            this._visita = visita;
            viewmodel = new InformeViewModel();
            BindingContext = viewmodel;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {

            var a = starFive.Rating;
            Stream image = await PadView.GetImageStreamAsync(SignatureImageFormat.Png);
            viewmodel.submit(image,a,_visita);

            // you can add code here to save the Stream image.
        }



    }
}