using GalaSoft.MvvmLight.Command;
using AppDemo.Models;
using AppDemo.Pages;
using AppDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using AppDemo.Helpers;
/// <summary>
/// View Model para para la vista de login 
/// Guarda los datos del usuario en las variables almacenadas en el local del telefono 
/// </summary>
namespace AppDemo.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Attributes
        public bool isRunning;
        #endregion

        #region Properties


        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get { return isRunning; }
        }



        public string Usuario { get; set; }

        public string Contrasena { get; set; }

        public bool Recuerdame { get; set; }
        #endregion

        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;


        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors
        public LoginViewModel()
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();

            Recuerdame = true;
        }

        #endregion

        #region Commands

        public ICommand NavigateRegisterCommand { get { return new RelayCommand(Navigate); } }

        private void Navigate()
        {
            // App.Current.MainPage = new RegisterPage();
            //await navigationService.Navigate("RegisterPage");
        }

        public ICommand LoginCommand { get { return new RelayCommand(Login); } }
        private async void Login()
        {
            IsRunning = true;
            if (string.IsNullOrEmpty(Usuario))
            {
                await dialogService.ShowMessage("Error", "Debe ingresar el nombre de Usuario");
                return;
            }

            if (string.IsNullOrEmpty(Contrasena))
            {
                await dialogService.ShowMessage("Error", "Debe ingresar la Contraseña");
                return;
            }

            var response = await apiService.Login();
            if (response.IsSuccess)
            {
                var agente = (Agente)response.Result;

                var agenteView = new AgenteViewModel
                {
                    Nombre = agente.Nombre,
                    Id = agente.Id,                  
                };

                var main = MainViewModel.GetInstance();
                main.LoadMenu(agenteView.Nombre);
                main.LoadClientes();

                Settings.userId = agente.Id;
                Settings.UserName = agente.Nombre;                
                Settings.companyId = 1;
                Settings.IsLoggedIn = true;
                navigationService.SetMainPage(agenteView);

                IsRunning = false;
                return;
            }

            await dialogService.ShowMessage("Error", "Usuario o contraseña incorrectos");
            IsRunning = false;
        }

        #endregion

    }
}
