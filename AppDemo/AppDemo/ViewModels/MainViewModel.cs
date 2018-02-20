﻿using GalaSoft.MvvmLight.Command;
using AppDemo.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Maps;
using System.ComponentModel;
using AppDemo.Models;
using AppDemo.Classes;
using Xamarin.Forms;
using System.Diagnostics;
using TK.CustomMap;
using Plugin.Geolocator;
using AppDemo.Pages;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Pages;
using AppDemo.Helpers;
using AppDemo.Constants;
using Plugin.Geolocator.Abstractions;
using TK.CustomMap.Overlays;
/// <summary>
/// Este es el View model principal desde aquí se inicializa la mayoria de las cosas
/// 
/// </summary>

namespace AppDemo.ViewModels
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
        public LoginViewModel NewLogin { get; set; }
        public AddViewModel AddnewClient { get; set; }
        public CheckinViewModel CheckinClient { get; set; }
        public ObservableCollection<Pin> Pins { get; set; }
        public ObservableCollection<PinRequest> LocationsRequest { get; set; }
        public ObservableCollection<TKCustomMapPin> locations;
        public TKCustomMapPin MyPin { get; set; }
        public TKRoute MyRoute { get; set; }
        public ObservableCollection<ListRequest> listlocation;
        public TKCustomMapPin myPosition;

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
            Pins = new ObservableCollection<Pin>();
            Locations = new ObservableCollection<TKCustomMapPin>();
            locations = new ObservableCollection<TKCustomMapPin>();
            listlocation = new ObservableCollection<ListRequest>();

            myPosition = new TKCustomMapPin();

            LocationsRequest = new ObservableCollection<PinRequest>();
            apiService = new ApiService();
            Menu = new ObservableCollection<MenuItemViewModel>();
            EncabezadoMenu = new MenuItemViewModel();
            
           
            navigationService = new NavigationService();
            NewLogin = new LoginViewModel();
            AddnewClient = new AddViewModel();
            CheckinClient = new CheckinViewModel();
            signalRService = new SignalRService();           
            LoadClientes();
            if (Settings.IsLoggedIn)
            {
                Locator();
            }
        }


        public async void LoadClientes()
        {           
                try
                {
                var clientes = await apiService.GetAllClients();
                Locations.Clear();
                ListLocation.Clear();
                clientes.Count();
                    if (clientes!=null && clientes.Count>0)
                    {
                        foreach (var cliente in clientes)
                        {
                            var Pincliente = new TKCustomMapPin
                            {
                                Image = "pin.png",
                                Position = new Xamarin.Forms.Maps.Position(cliente.Lat, cliente.Lon),
                                Title = cliente.Nombre,
                                Subtitle = "Dirección: "+cliente.Direccion,
                                
                                ShowCallout = true,
                            };
                            var itemcliente = new ListRequest
                            {
                                Titulo=cliente.Nombre,
                                Subtitulo= cliente.PersonaContacto+" "+ cliente.Telefono,                                                     
                            };
                            Locations.Add(Pincliente);
                        ListLocation.Add(itemcliente);
                        }
                     }
                }
                catch
                {

                }
            }
    
        public ObservableCollection<TKCustomMapPin> Locations
    {
            protected set
            {
                locations = Locations;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Locations"));
                
            }
            get { return locations; }
        }
        public ObservableCollection<ListRequest> ListLocation
        {
            protected set
            {
                listlocation = ListLocation;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ListLocation"));

            }
            get { return listlocation; }
        }


        /// <summary>
        /// Desde aquí enviamos los datos de localización de forma periodica hacia la aplicacion web 
        /// </summary>
        /// <returns></returns>
        private async Task Locator()
        {
            await CrossGeolocator.Current.StartListeningAsync(3, 10, true);
            CrossGeolocator.Current.PositionChanged += CrossGeolocator_Current_PositionChanged;        
        }
       

        async  void CrossGeolocator_Current_PositionChanged(object sender, PositionEventArgs e)
        {
            
            Device.BeginInvokeOnMainThread(() =>
            {
                var position = e.Position;
                myPosition.Position = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);

                //e.Position.Latitude,0);
                
                // e.Position.Longitude;
            });

           await apiService.PostLogPosition(new LogPosition { idAgente = App.AgenteActual.Id, Lat = e.Position.Latitude, Lon = e.Position.Longitude, Fecha=DateTime.Now });
           await  signalRService.SendPosition((float)e.Position.Latitude, (float)e.Position.Longitude);

        }


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

        public ICommand PinSelected { get { return new RelayCommand(pinselected); } }

        public async void pinselected()
        {

            //Debug.WriteLine("debe pintarse la ruta" + MyPin.Position.Latitude);

            //MyRoute = new TKRoute
            //{
            //    TravelMode = TKRouteTravelMode.Driving,
            //    Source = myPosition.Position,
            //    Destination = MyPin.Position,
            //    Color = Color.Blue
            //};

        }

        public ICommand RefreshDataCommand { get { return new RelayCommand(RefreshData); } }
        public void RefreshData()
        {
            LoadClientes();
        }

     //   public ICommand RefreshParkingCommand { get { return new RelayCommand(RefreshData); } }
      
            
        

       


        public ICommand AddNewClientCommand { get { return new RelayCommand(AddNewClient); } }
        public async void AddNewClient()
        {
            PopupPage page = new AddPage();
            await PopupNavigation.PushAsync(page);
        }

        public ICommand AddCheckinCommand { get { return new RelayCommand(AddCheckin); } }
        public async void AddCheckin()
        {
            PopupPage page = new CheckinPage();

          
            await PopupNavigation.PushAsync(page);

        }

       

        #endregion

        #region Methods
        /// <summary>
        /// Carga los vehiculos multados por el agente actual en la fecha actual para cuando sea necesario buscarlos
        /// </summary>
        //public async void LoadVehiculosMultados()
        //{
        //    IsRunning = true;
        //    var vehiculos = await apiService.loadVehiculosMultados(navigationService.GetAgenteActual().Id.ToString());
        //    VehiculosMultados.Clear();
        //    foreach (var vehiculo in vehiculos)
        //    {
        //        VehiculosMultados.Add(new VehiculosMultadosViewModels
        //        {
        //            Valor = vehiculo.AgenteId,
        //            Agente = vehiculo.Agente,
        //            AgenteId = vehiculo.AgenteId,
        //            latitud = vehiculo.latitud,
        //            Longitud = vehiculo.Longitud,
        //            Placa = vehiculo.Placa,
        //            Fecha = vehiculo.Fecha,
        //            Foto = vehiculo.Foto,
        //            MultaId = vehiculo.MultaId,
        //        }
        //        );
        //    }
        //    IsRunning = false;
        //}
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
                Icon = "addc.png",
                Title = "Nuevo Cliente",
                SubTitle = "",

            });

            Menu.Add(new MenuItemViewModel
            {

                PageName = "ConsultarMultas",
                Icon = "checkin.png",
                Title = "Checkin",
                SubTitle = "",
            });

            EncabezadoMenu.Agente = Agente;
        }
        #endregion


    }
}
