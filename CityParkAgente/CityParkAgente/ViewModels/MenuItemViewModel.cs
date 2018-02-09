using GalaSoft.MvvmLight.Command;
using CityParkAgente.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using CityParkAgente.Helpers;
using CityParkAgente.Pages;

namespace CityParkAgente.ViewModels
{
   public class MenuItemViewModel
    {
        #region Attributes

        private NavigationService navigationService;

        #endregion

        #region Properties

        public string Agente { get; set; }

        public string Icon { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string PageName { get; set; }
        #endregion

        #region Constructors

        public MenuItemViewModel()
        {
            navigationService = new NavigationService();
        }
        #endregion

        #region Commands

        public ICommand NavigateCommand { get {return new RelayCommand(Navigate) ; } }

        private async void Navigate()
        {
            await navigationService.Navigate(PageName);
        }

        public ICommand LogoutCommand { get { return new RelayCommand(Logout); } }
        private void Logout()
        {
            Settings.IsLoggedIn = false;
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

        public ICommand PasswordCommand { get { return new RelayCommand(Password); } }
        private async void Password()
        {
            await navigationService.Navigate("PasswordPage");
        }

        #endregion


    }
}
