using AppDemo.Classes;
using AppDemo.Services;
using Plugin.Geolocator;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TK.CustomMap;
using TK.CustomMap.Overlays;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;


namespace AppDemo.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        NavigationService navigationService = new NavigationService();
        ApiService apiService = new ApiService();
        ObservableCollection<TKPolygon> poligonos = new ObservableCollection<TKPolygon>();
        // Para bindear los pins
     //   public ObservableCollection<TKCustomMapPin> Locations { get; set; }
     //   public ObservableCollection<PinRequest> LocationsRequest { get; set; }

        public ICommand PinCommand;

        public MainPage()
        {
            InitializeComponent();
           // Locations = new ObservableCollection<TKCustomMapPin>();
          //  LocationsRequest = new ObservableCollection<PinRequest>();
            try
            {
                Locator();
               // CargarLugares();
            }
            catch
            {
            }
        }
        async void Locator()
        {

            
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 25;
            var location = await locator.GetPositionAsync();
            // comentado la posicion real para hacer pruebas en Cuenca
            var position = new Position(location.Latitude, location.Longitude);
            // var position = new Position(-2.908721,-79.038660);
            await Task.Delay(3000);
            Mapa.MoveToRegion(MapSpan.FromCenterAndRadius((position), Distance.FromMiles(.3)));
            await Task.Delay(3000);
            Mapa.MoveToMapRegion((MapSpan.FromCenterAndRadius(position, Distance.FromMiles(.3))), true);
            // Mapa.Polygons;

            var cordenadas = await apiService.GetMyPolygon(navigationService.GetAgenteActual().AgenteId);
            if (cordenadas.Count() > 0)
            {
                // _____________________________
                var poly = new TKPolygon
                {
                    StrokeColor = Color.Blue,
                    StrokeWidth = 2f,
                    Color = new Color(0, 0, 255, 0.1),
                    Coordinates = cordenadas,
                };
                poligonos.Add(poly);

                Mapa.Polygons = poligonos;

                // _____________________________
            }
            // Mapa.MapCenter = position;
            // Mapa.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(.3)));
            // Mapa.MapRegion=(new MapSpan(position, 0.1, 0.1));

            //  Mapa = new TKCustomMap(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(2)));
        }

        //public async void CargarLugares()
        //{
        //    Locations.Clear();

        //    if (navigationService.GetAgenteActual() != null)
        //    {
        //        LocationsRequest = await apiService.GetParqueados();
        //        if (LocationsRequest != null && LocationsRequest.Count() > 0)
        //        {
        //            foreach (var location in LocationsRequest)
        //            {
        //                string minuto = "" + location.HoraFin.ToLocalTime().Minute;
        //                string iconPin = "auto.png";
        //                TimeSpan tiempoSobrante = location.HoraFin.ToLocalTime() - DateTime.Now.ToLocalTime();
        //                Debug.WriteLine(tiempoSobrante);
        //                if (tiempoSobrante < new TimeSpan(0, 5, 99))
        //                {
        //                    iconPin = "autorojo.png";
        //                }

        //                if (location.HoraFin.ToLocalTime().Minute.ToString().Length == 1)
        //                {
        //                    minuto = "0" + location.HoraFin.ToLocalTime().Minute;
        //                }

        //                var pin = new TKCustomMapPin
        //                {
        //                    Image = iconPin,
        //                    Position = new Position(location.Latitud, location.Longitud),
        //                    Title = location.placa,
        //                    Subtitle = "El parqueo finaliza a las" + location.HoraFin.ToLocalTime().Hour + ":" + minuto,
        //                    ShowCallout = true,
        //                };
        //                Locations.Add(pin);
        //                Mapa.CustomPins = Locations;
        //            }
        //        }
        //    }
        //}
    }
}