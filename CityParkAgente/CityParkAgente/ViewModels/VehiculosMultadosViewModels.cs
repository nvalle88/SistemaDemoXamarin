using CityParkAgente.Models;
using CityParkAgente.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CityParkAgente.ViewModels
{
 public class VehiculosMultadosViewModels:Multa
    {
        NavigationService navigationService;
        DialogService dialogService;
        public VehiculosMultadosViewModels()
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
        }
        public ICommand EliminarCommand { get { return new RelayCommand(Emlinar); } }

        private async void Emlinar()
        {
            await dialogService.ShowMessage("Ok", "Ok");
        }
    }
}
