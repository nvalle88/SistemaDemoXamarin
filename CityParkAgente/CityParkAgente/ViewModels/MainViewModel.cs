﻿using GalaSoft.MvvmLight.Command;
using CityParkAgente.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Maps;
using System.ComponentModel;
using CityParkAgente.Models;
using CityParkAgente.Classes;
using Xamarin.Forms;
using System.Diagnostics;
using TK.CustomMap;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using TK.CustomMap.Overlays;
using CityParkAgente.Pages;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Pages;
using CityParkAgente.Helpers;
using CityParkAgente.Constants;
/// <summary>
/// Este es el View model principal desde aquí se inicializa la mayoria de las cosas
/// 
/// </summary>

namespace CityParkAgente.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
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


        #region Singleton

        static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }

        #endregion

        #region Propeties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public MenuItemViewModel EncabezadoMenu { get; set; }

        public ObservableCollection<VehiculosMultadosViewModels> VehiculosMultados { get; set; }
        public LoginViewModel NewLogin { get; set; }
        public PonerMultaViewModel NuevaMulta { get; set; }
        public ObservableCollection<Pin> Pins { get; set; }
        public VerificarAutoViewModel VerificarAuto { get; set; }
        public PasswordViewModel NewPasword { get; set; }

        public ObservableCollection<PinRequest> LocationsRequest { get; set; }
        public ObservableCollection<TKCustomMapPin> locations;



        public ICommand PinCommand;

        public bool IsRefreshing
        {
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshing"));
                }
            }
            get
            {
                return isRefreshing;
            }
        }


        #endregion

        #region Attributes
        private bool isRefreshing = false;
        private NavigationService navigationService;
        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Services

        private ApiService apiService;
        private SignalRService signalRService;

        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            VehiculosMultados = new ObservableCollection<VehiculosMultadosViewModels>();
            Pins = new ObservableCollection<Pin>();
            Locations = new ObservableCollection<TKCustomMapPin>();
            locations = new ObservableCollection<TKCustomMapPin>(); 

            LocationsRequest = new ObservableCollection<PinRequest>();
            apiService = new ApiService();
            Menu = new ObservableCollection<MenuItemViewModel>();
            EncabezadoMenu = new MenuItemViewModel();
            if (Settings.IsLoggedIn)
            {
                NuevaMulta = new PonerMultaViewModel();
            }
            navigationService = new NavigationService();
            NewLogin = new LoginViewModel();
            VerificarAuto = new VerificarAutoViewModel();
            signalRService = new SignalRService();
            NewPasword = new PasswordViewModel();
            CargarLugares();
        }
        public async void InitMultas()
        {
            NuevaMulta = new PonerMultaViewModel();
        }



        public async void CargarLugares()
        {
            try
            {
                Locations.Clear();
                IsRunning = true;
                if (navigationService.GetAgenteActual() != null)
                {
                    LocationsRequest = await apiService.GetParqueados();
                    if (LocationsRequest != null && LocationsRequest.Count() > 0)
                    {
                        foreach (var location in LocationsRequest)
                        {
                            string minuto = "" + location.HoraFin.ToLocalTime().Minute;
                            string iconPin = "auto.png";
                            TimeSpan tiempoSobrante = location.HoraFin.ToLocalTime() - DateTime.Now.ToLocalTime();
                            Debug.WriteLine(tiempoSobrante);
                            if (tiempoSobrante < new TimeSpan(0, 5, 99))
                            {
                                iconPin = "autorojo.png";
                            }

                            if (location.HoraFin.ToLocalTime().Minute.ToString().Length == 1)
                            {
                                minuto = "0" + location.HoraFin.ToLocalTime().Minute;
                            }
                            var pin = new TKCustomMapPin
                            {
                                Image = iconPin,

                                Position = new Position(location.Latitud, location.Longitud),
                                Title = location.placa,
                                Subtitle = "El parqueo finaliza a las" + location.HoraFin.ToLocalTime().Hour + ":" + minuto,
                                ShowCallout = true,

                            };
                            Locations.Add(pin);
                        }
                    }
                   await Locator();
                    Device.StartTimer(TimeSpan.FromSeconds(Constants.Constants.TimeForSignalR), () =>
                    {
                         Locator();
                        return true;
                    });
                }
            }
            catch
            {

            }
        }
       // public { get; set; }

        public ObservableCollection<TKCustomMapPin> Locations
    {
            protected set
            {
                locations = Locations;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Locations"));
                
            }
            get { return locations; }
        }

        /// <summary>
        /// Desde aquí enviamos los datos de localización de forma periodica hacia la aplicacion web 
        /// </summary>
        /// <returns></returns>
        private async Task Locator()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 25;
            var location = await locator.GetPositionAsync();
            await Task.Delay(1000);
            signalRService.SendPosition((float)location.Latitude, (float)location.Longitude);
        }







        //    var locations = new List<BindableLocation47>();

        //    var location = new BindableLocation47
        //    {

        //        LocationTitle = "Usuario 1",

        //        LocationDescription = "Su tiempo de parqueo termina a las 17:30 ",

        //        Latitude = -0.174791,

        //        Longitude = -78.483573,

        //        ActionCommand = new Command(PinSelected)

        //    };           

        //    locations.Add(location);
        //    location = new BindableLocation47

        //    {

        //        LocationTitle = "Usuario 2",

        //        LocationDescription = "Su tiempo de parqueo termina a las 15:00",

        //        Latitude = -0.173200,

        //        Longitude = -78.483307,

        //        ActionCommand = new Command(PinSelected)

        //    };
        //    locations.Add(location);
        //    location = new BindableLocation47

        //    {

        //        LocationTitle = "Usuario 3",

        //        LocationDescription = "Su tiempo de parqueo termina a las 16:30",

        //        Latitude = -0.175324,

        //        Longitude = -78.485356,

        //        ActionCommand = new Command(PinSelected)



        //    };
        //    locations.Add(location);         
        //    Locations = new ObservableCollection<BindableLocation47>(locations);


        //}
        //public void PinSelected(object param)

        //{

        //    var pin = param as Pin;



        //    if (pin != null)

        //    {



        //         Debug.WriteLine(pin.Label);



        //    }

        //}


        #endregion

        #region Commands
        /// <summary>
        /// En esta Región se encuantran los commands que estan realcionados con los botones de las vistas
        /// </summary>
        public ICommand VerificarAutoCommand { get { return new RelayCommand(VerificarAutonavigate); } }
        public async void VerificarAutonavigate()
        {
            await navigationService.Navigate("VerificarAutoPage");
            IsRefreshing = false;
        }

        public ICommand ConsultarMultasCommand { get { return new RelayCommand(ConsultarMultas); } }

        public async void ConsultarMultas()
        {
            await navigationService.Navigate("ConsultarMultas");
            IsRefreshing = false;
        }

        public ICommand RefreshCarrosCommand { get { return new RelayCommand(RefreshCarros); } }
        public void RefreshCarros()
        {
            IsRefreshing = false;
        }

        public ICommand RefreshParkingCommand { get { return new RelayCommand(RefreshParking); } }
        public void RefreshParking()
        {
            CargarLugares();
        }

        public ICommand ViewListCommand { get { return new RelayCommand(ViewList); } }
        public async void ViewList()
        {
            PopupPage page = new ListViewPage();
            await PopupNavigation.PushAsync(page);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Carga los vehiculos multados por el agente actual en la fecha actual para cuando sea necesario buscarlos
        /// </summary>
        public async void LoadVehiculosMultados()
        {
            IsRunning = true;
            var vehiculos = await apiService.loadVehiculosMultados(navigationService.GetAgenteActual().AgenteId.ToString());
            VehiculosMultados.Clear();
            foreach (var vehiculo in vehiculos)
            {
                VehiculosMultados.Add(new VehiculosMultadosViewModels
                {
                    Valor = vehiculo.AgenteId,
                    Agente = vehiculo.Agente,
                    AgenteId = vehiculo.AgenteId,
                    latitud = vehiculo.latitud,
                    Longitud = vehiculo.Longitud,
                    Placa = vehiculo.Placa,
                    Fecha = vehiculo.Fecha,
                    Foto = vehiculo.Foto,
                    MultaId = vehiculo.MultaId,
                }
                );
            }
            IsRunning = false;
        }
        /// <summary>
        /// Se arma el menu y en el encabezado del menú se muestra el nombre del agente logeado en la aplicación
        /// </summary>
        /// <param name="Agente"></param>
        public void LoadMenu(string Agente)
        {
            Menu.Clear();
            Menu.Add(new MenuItemViewModel
            {

                PageName = "VerificarAutoPage",
                Icon = "ic_Recarga_Prepago.png",
                Title = "Verificar vehículo",
                SubTitle = "",

            });

            Menu.Add(new MenuItemViewModel
            {

                PageName = "ConsultarMultas",
                Icon = "ic_Historial_Compras.png",
                Title = "Consultar multas del día",
                SubTitle = "",
            });

            EncabezadoMenu.Agente = Agente;
        }
        #endregion
    }
}