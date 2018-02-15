using AppDemo.Helpers;
using AppDemo.Models;
using AppDemo.Pages;
using AppDemo.Services;
using GalaSoft.MvvmLight.Command;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppDemo.ViewModels
{
   public  class CheckinViewModel: INotifyPropertyChanged
    {
        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        public event PropertyChangedEventHandler PropertyChanged;
        public Visita visita { get; set; }
        public List<Cliente> cliente;  
        public List<Cliente> Cliente
        {
            set
            {
                if (cliente != value)
                {
                    cliente = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Cliente"));
                }
            }
            get
            {
                return cliente;
            }

        }

        public List<Tipos> Tipos { get; set; }
        public string valor { get; set; }

        #endregion

        #region Properties
        public Helpers.GeoUtils.Position position { get; set; }

        #endregion

        #region constructor
        public  CheckinViewModel()
        {
            valor ="";
            position = new Helpers.GeoUtils.Position();
            visita = new Visita();
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
            if (Settings.IsLoggedIn)
            {
                init();
            }
            Tipos = new List<Models.Tipos>
            {
             new Tipos{id=1,tipo="Venta"},
             new Tipos{id=2,tipo="Visita"}
            };
        }
        #endregion
        private async Task init()
        {
           
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 25;
            var location = await locator.GetPositionAsync();
            position.latitude = location.Latitude;
            position.longitude = location.Longitude;
            Cliente = await apiService.GetNearClients(position);
            
        }

        Cliente clienteSelect;
        public Cliente clienteSelectItem
        {
            get
            {
                return clienteSelect;
            }
            set
            {
                // marcaseleccionada = ;

                clienteSelect = value;
            }
        }

        Tipos tipoSelect;
        public Tipos TipoSelectItem
        {
            get
            {
                return tipoSelect;
            }
            set
            {
                // marcaseleccionada = ;

                tipoSelect = value;
            }
        }

        public ICommand CheckCommand { get { return new RelayCommand(Checkin); } }
        private async void Checkin()
        {


            visita.IdCliente = clienteSelect.Id;
            visita.Tipo = tipoSelect.id;
            visita.IdAgente = App.AgenteActual.Id;
            visita.Fecha = DateTime.Now;
            visita.Valor = Double.Parse(valor);

            if(visita!=null)
            {
               var result= await apiService.Checkin(visita);
                if (result.IsSuccess)
                {
                    await dialogService.ShowMessage("Checkin", "Se agrego su visita correctamente");
                    await PopupNavigation.PopAllAsync();

                }
            }
            
        }


        public ICommand CloseCommand { get { return new RelayCommand(Close); } }
        public async void Close()
        {
        //    PopupPage page = new CheckinPage();
            
            await PopupNavigation.PopAllAsync();
        }




    }
}
