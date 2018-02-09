using GalaSoft.MvvmLight.Command;
using CityParkAgente.Models;
using CityParkAgente.Pages;
using CityParkAgente.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using CityParkAgente.Helpers;
/// <summary>
/// View Model para para la vista de login 
/// Guarda los datos del usuario en las variables almacenadas en el local del telefono 
/// </summary>
namespace CityParkAgente.ViewModels
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

            var response = await apiService.Login(Usuario, Contrasena);
            if (response.IsSuccess)
            {
                var agente = (Agente)response.Result;

                var agenteView = new AgenteViewModel
                {
                    Contrasena = agente.Contrasena,
                    Nombre = agente.Nombre,
                    AgenteId = agente.AgenteId,
                    Apellido = agente.Apellido,
                    Multa = agente.Multa,
                };

                var main = MainViewModel.GetInstance();
                main.LoadMenu(agenteView.Nombre);
                main.CargarLugares();

                Settings.userId = agente.AgenteId;
                Settings.UserName = agente.Nombre;
                Settings.UserLastName = agente.Apellido;
                Settings.companyId = agente.EmpresaId;
                Settings.IsLoggedIn = true;
                main.InitMultas();
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
