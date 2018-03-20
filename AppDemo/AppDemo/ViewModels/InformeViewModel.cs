using AppDemo.Helpers;
using AppDemo.Models;
using AppDemo.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
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


    public class InformeViewModel
    {
        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        public event PropertyChangedEventHandler PropertyChanged;
        public Informe informe { get; set; }

    

        #endregion

        #region Properties
        public Helpers.GeoUtils.Position position { get; set; }

        #endregion

        #region constructor
        public InformeViewModel()
        {
            position = new Helpers.GeoUtils.Position();
            informe = new Informe();
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();                    
        }
        #endregion

        #region Command
        public async void submit(Stream image, int calificacion, Visita visita)
        {
            Debug.WriteLine(informe.SolucionComercial);
            var imagen=   await apiService.SetFirmaAsync(visita.Id, image);
            informe.UrlFirma = imagen.Message;
            informe.Calificacion = calificacion;
            informe.IdVisita = visita.Id;
            
            var a = await apiService.SubirInforme(informe);            
            if(a.IsSuccess)
            {
                await dialogService.ShowMessage("Informe", "Se guardo correctamente");
                await navigationService.Navigate("MainPage");
            }
        }

        #endregion

    }
}
