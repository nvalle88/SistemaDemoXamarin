using AppDemo.Classes;
using AppDemo.Helpers;
using AppDemo.Models;
using AppDemo.Pages;
using AppDemo.Services;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;



namespace AppDemo.ViewModels
{
    public class PonerMultaViewModel : Multa, INotifyPropertyChanged
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


        public event PropertyChangedEventHandler PropertyChanged;

        private MediaFile file;

        private ImageSource imageSource;

        public ImageSource ImageSource
        {
            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource"));
                }
            }
            get
            {
                return imageSource;
            }
        }


        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        public ObservableCollection<TipoMultas> tipoMultas { get; set; }
        public CarroRequest Carro { get; set; }

        public string observacion { get; set; }

        public PonerMultaViewModel()
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
            tipoMultas = new ObservableCollection<TipoMultas>();
            Carro = new CarroRequest();
            observacion = string.Empty;
            tipodeMultas();
        }


        public async void tipodeMultas()
        {
            var tipoMultas = await apiService.loadTipoDeMultas();
            this.tipoMultas.Clear();
            foreach (var tmultas in tipoMultas)
            {
                this.tipoMultas.Add(tmultas);
            }

        }
        TipoMultas MultaSeleccionada;
        public TipoMultas MultaSelectedItem
        {
            get
            {
                return MultaSeleccionada;
            }
            set
            {
                // marcaseleccionada = ;
                MultaSeleccionada = value;
            }
        }


        public ICommand TakePictureCommand { get { return new RelayCommand(TakePicture); } }

        private async void TakePicture()
        {
            await Plugin.Media.CrossMedia.Current.Initialize();

            if (!Plugin.Media.CrossMedia.Current.IsCameraAvailable || !Plugin.Media.CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "Aceptar");
            }

            file = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Photos",
                Name = "NuevaMulta.jpg",
                PhotoSize = PhotoSize.Small,
                DefaultCamera = CameraDevice.Rear,
                CompressionQuality = 50,
                SaveToAlbum = true,


                //CompressionQuality=80,                        



            });







            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();

                    return stream;
                });
            }
        }


        public ICommand MultarCommand { get { return new RelayCommand(Multar); } }



        private async void Multar()
        {
            IsRunning = true;
            if (file == null)
            {
                await dialogService.ShowMessage("Error", "Debe tomar la foto...");
                IsRunning = false;
                return;
            }

            if (string.IsNullOrEmpty(Placa))
            {
                await dialogService.ShowMessage("Error", "Debe ingresar la placa del vehículo");
                IsRunning = false;
                return;
            }

            if ((MultaSeleccionada == null || MultaSeleccionada.TipoMultaId == 0))
            {
                await dialogService.ShowMessage("Error", "Debe elegir un tipo de multa ...");
                IsRunning = false;
                return;
            }
            var PP = PonerMultaPage.GetInstance();

            var multa = new Multa
            {
                AgenteId = navigationService.GetAgenteActual().Id,
                Fecha = DateTime.Now,
                Valor = Valor,
                Placa = Placa,
                latitud = PP.Location.Latitude,
                Longitud = PP.Location.Longitude,
                Plaza = Plaza,
                EmpresaId = Settings.companyId,
                TipoMultaId = MultaSeleccionada.TipoMultaId,
                Observacion = observacion,

            };
            var response = await apiService.InsertarMulta(multa);

            if (response.IsSuccess && file != null)
            {
                var newMulta = (Multa)response.Result;
                var response2 = await apiService.SetPhotoAsync(newMulta.MultaId, file.GetStream());
                var filenName = string.Format("{0}.jpg", newMulta.MultaId);
                var folder = "~/Content/Multas";
                var fullPath = string.Format("{0}/{1}", folder, filenName);
                multa.Foto = fullPath;
            }
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                IsRunning = false;
            }
            var respuestaMulta = (Multa)response.Result;
            await dialogService.ShowMessage("CityPark", string.Format("Multa insertada correctamente Placa: {0}", multa.Placa));
            navigationService.SetMainPage(navigationService.GetAgenteActual());
            IsRunning = false;
        }
    }
}
