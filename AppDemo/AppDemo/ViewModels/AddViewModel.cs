﻿using AppDemo.Models;
using AppDemo.Services;
using GalaSoft.MvvmLight.Command;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppDemo.ViewModels
{
    public class AddViewModel: INotifyPropertyChanged
    {
        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
       public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public Cliente cliente { get; set; }

        #endregion
        #region constructor
        public AddViewModel()
        {
            cliente = new Cliente();
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
        }
        #endregion
        private async Task Locator()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var location = await locator.GetPositionAsync();
            cliente.Lat = location.Latitude;
            cliente.Lon = location.Longitude;
        }

        public ICommand AddCommand { get { return new RelayCommand(Add); } }
        private async void Add()
        {
            await Locator();

            if (string.IsNullOrEmpty(cliente.Nombre))
            {
                await dialogService.ShowMessage("Error", "Debe ingresar el nombre del cliente");
                return;
            }

            if (string.IsNullOrEmpty(cliente.Telefono))
            {
                await dialogService.ShowMessage("Error", "Debe ingresar el telefono del cliente");
                return;
            }

            var response = await apiService.postNewClient(cliente);
            if (response.IsSuccess)
            {
                var cliente = (Cliente)response.Result;
                await dialogService.ShowMessage("Ok", "Cliente registrado correctamente");
                //  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Locations"));
                cliente = new Cliente();
                await PopupNavigation.PopAllAsync();
                return;
            }

            await dialogService.ShowMessage("Error", "Cliente no registrado");
        }

        public ICommand CloseCommand { get { return new RelayCommand(Close); } }
        public async void Close()
        {
            //    PopupPage page = new CheckinPage();
            await PopupNavigation.PopAllAsync();
        }


    }
}
