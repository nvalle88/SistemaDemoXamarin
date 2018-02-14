using AppDemo.Models;
using AppDemo.Services;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.ViewModels
{
   public  class CheckinViewModel: INotifyPropertyChanged
    {
        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        public event PropertyChangedEventHandler PropertyChanged;
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

        #endregion

        #region Properties
        public Helpers.GeoUtils.Position position { get; set; }

        #endregion

        #region constructor
        public  CheckinViewModel()
        {
            position = new Helpers.GeoUtils.Position();

            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
            init();
        }
        #endregion
        private async Task init()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 25;
            var location = await locator.GetPositionAsync();
            position.latitude = location.Latitude;
            position.longitude = location.Longitude;
            Cliente= await apiService.GetNearClients(position);

        }
    }
}
