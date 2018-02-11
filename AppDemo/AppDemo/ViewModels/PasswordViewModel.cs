using AppDemo.Classes;
using AppDemo.Helpers;
using AppDemo.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
/// <summary>
/// ViewModel para el cambio de contraseña 
/// </summary>
namespace AppDemo.ViewModels
{
    public class PasswordViewModel : INotifyPropertyChanged
    {
        #region Properties
        public string Agente { get; set; }

        public string ContrasenaActual { get; set; }
        public string ContrasenaNueva { get; set; }
        public string ContrasenaNueva2 { get; set; }


        public AgenteViewModel UsuarioActual { get; set; }
        public bool isRunning;
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
        #endregion

        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors
        public PasswordViewModel()
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
            UsuarioActual = new AgenteViewModel();
            Agente = Settings.UserName;
        }

        #endregion

        #region Commands
        public ICommand updateCommand { get { return new RelayCommand(updatePassword); } }
        private async void updatePassword()
        {
            if (ContrasenaActual != string.Empty && ContrasenaNueva != string.Empty && ContrasenaNueva2 != string.Empty)
            {
                    if (ContrasenaNueva == ContrasenaNueva2)
                    {
                        UsuarioPasswordRequest usuario = new UsuarioPasswordRequest
                        {
                            UsuarioId = Settings.userId,
                          
                            Contrasena=ContrasenaActual,
                            ContrasenaNueva= ContrasenaNueva,
                        };
                        var response = await apiService.UpdatePasword(usuario);
                        if (response.IsSuccess)
                        {
                            var newPassword = (Models.Usuario)response.Result;
                            await dialogService.ShowMessage(" Contraseña Modificada", "la contraseña fue modificada con exito.");
                            IsRunning = false;
                            navigationService.SetMainPage(navigationService.GetAgenteActual());
                            return;
                        }
                    else
                    {

                        await dialogService.ShowMessage("Error", response.Message);

                    }


                }
                else
                    {
                        await dialogService.ShowMessage("Error", "La nueva contraseña no coincide");
                    }                               
            }
            else
            {
                await dialogService.ShowMessage("Error", "Debe ingresar las Contraseñas");

            }

        }
        #endregion

    }
}
