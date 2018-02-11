using AppDemo.Classes;
using AppDemo.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppDemo.ViewModels
{
   public class VerificarAutoViewModel:INotifyPropertyChanged
    {

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
        public string Placa { get; set; }
        public string Plaza { get; set; }
        private ApiService apiService;
        private NavigationService navigationService;
        private DialogService dialogService;
        public event PropertyChangedEventHandler PropertyChanged;
        public VerificarAutoViewModel()
        {
            apiService = new ApiService();
            navigationService = new NavigationService();
            dialogService = new DialogService();
        }
        public ICommand VerificarAutoCommand { get { return new RelayCommand(VerificarAutosAsync); } }
        private async  void VerificarAutosAsync()
        {
            IsRunning = true;
            if (string.IsNullOrEmpty(Placa))
            {
                await dialogService.ShowMessage("Información", "Debe introducir la placa del vehículo que desea verificar...");
                IsRunning = false;
                return;
            }
            if (string.IsNullOrEmpty(Plaza))
            {
                await dialogService.ShowMessage("Información", "Debe introducir la plaza en la que se encuentra el vehículo a verificar...");
                IsRunning = false;
                return;
            }            
            apiService.VerificarAuto(Placa,Plaza);                       
            IsRunning = false;
             
        }

    }


}
