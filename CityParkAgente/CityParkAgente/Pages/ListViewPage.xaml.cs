using CityParkAgente.Classes;
using CityParkAgente.Services;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms.Xaml;

namespace CityParkAgente.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewPage : PopupPage
    {
        // Para bindear los pins
        ApiService apiService;
        NavigationService navigationService;

        public ObservableCollection<PinRequest> LocationsRequest { get; set; }
        public ObservableCollection<ListRequest> Locations { get; set; }

        public ListViewPage()
        {
            InitializeComponent();
            LocationsRequest = new ObservableCollection<PinRequest>();
            Locations = new ObservableCollection<ListRequest>();
            navigationService = new NavigationService();
            apiService = new ApiService();

            CargarLugares();

            BindingContext = this;
        }

        async void OnClose(object sender, EventArgs e) => await Navigation.PopPopupAsync();

        public async void CargarLugares()
        {
            Locations.Clear();

            if (navigationService.GetAgenteActual() != null)
            {
                LocationsRequest = await apiService.GetParqueados();
                if (LocationsRequest != null && LocationsRequest.Count() > 0)
                {
                    foreach (var location in LocationsRequest)
                    {
                        string minuto = "" + location.HoraFin.ToLocalTime().Minute;
                        TimeSpan tiempoSobrante = location.HoraFin.ToLocalTime() - DateTime.Now.ToLocalTime();

                        if (location.HoraFin.ToLocalTime().Minute.ToString().Length == 1)
                        {
                            minuto = "0" + location.HoraFin.ToLocalTime().Minute;
                        }

                        var item = new ListRequest
                        {
                            Titulo = location.placa,
                            Subtitulo = "Finaliza a las " + location.HoraFin.ToLocalTime().Hour + ":" + minuto,
                        };
                        Locations.Add(item);
                    }
                }
            }
        }
    }
}