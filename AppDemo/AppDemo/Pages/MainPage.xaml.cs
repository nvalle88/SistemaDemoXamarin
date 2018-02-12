using AppDemo.Classes;
using AppDemo.Services;
using Plugin.Geolocator;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
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

        private static readonly CompositeDisposable EventSubscriptions = new CompositeDisposable();
        private readonly PanGestureRecognizer _panGesture = new PanGestureRecognizer();
        private double _transY;

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

            var cordenadas = await apiService.GetMyPolygon(navigationService.GetAgenteActual().Id);
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitializeObservables();
            CollapseAllMenus();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            EventSubscriptions.Clear();
        }

        private void CollapseAllMenus()
        {
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(200);
                Device.BeginInvokeOnMainThread(() =>
                {
                    Notification.HeightRequest = this.Height - QuickMenuLayout.Height;
                    QuickMenuPullLayout.TranslationY = Notification.HeightRequest;
                });
            });
        }

        private void InitializeObservables()
        {
            //IF THERE IS OBSERVABLES
            var panGestureObservable = Observable
                .FromEventPattern<PanUpdatedEventArgs>(
                    x => _panGesture.PanUpdated += x,
                    x => _panGesture.PanUpdated -= x
                )
                //.Throttle(TimeSpan.FromMilliseconds(20), TaskPoolScheduler.Default)
                .Subscribe(x => Device.BeginInvokeOnMainThread(() => { CheckQuickMenuPullOutGesture(x); }));

            EventSubscriptions.Add(panGestureObservable);
            QuickMenuInnerLayout.GestureRecognizers.Add(_panGesture);
        }

        private void CheckQuickMenuPullOutGesture(EventPattern<PanUpdatedEventArgs> x)
        {
            var e = x.EventArgs;
            var typeOfAction = x.Sender as StackLayout;

            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    MethodLockedSync(() =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            QuickMenuPullLayout.TranslationY = Math.Max(0,
                                Math.Min(Notification.HeightRequest, QuickMenuPullLayout.TranslationY + e.TotalY));
                        });
                    }, 2);

                    break;

                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    _transY = QuickMenuPullLayout.TranslationY;
                    break;
                case GestureStatus.Canceled:
                    Debug.WriteLine("Canceled");
                    break;
            }
        }

        private CancellationTokenSource _throttleCts = new CancellationTokenSource();
        private void MethodLockedSync(Action method, double timeDelay = 500)
        {
            Interlocked.Exchange(ref _throttleCts, new CancellationTokenSource()).Cancel();
            Task.Delay(TimeSpan.FromMilliseconds(timeDelay), _throttleCts.Token) // throttle time
                .ContinueWith(
                    delegate { method(); },
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }

    }
}