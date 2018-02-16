using AppDemo.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppDemo.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckinPage : PopupPage

    {
        CheckinViewModel viewmodel;
        public CheckinPage ()
		{
			InitializeComponent ();
            viewmodel = new CheckinViewModel();
            BindingContext = viewmodel;
        }
    }
}