using AppDemo.Helpers;
using AppDemo.Pages;
using AppDemo.Services;
using AppDemo.ViewModels;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace AppDemo
{
    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }
        public static AgenteViewModel AgenteActual { get; internal set; }
        public App()
        {
            InitializeComponent();
            // MainPage = new LoginPage();
            if (CrossConnectivity.Current.IsConnected)
            {
                if (Settings.IsLoggedIn)
                {
                    AgenteViewModel agenteView = new AgenteViewModel
                    {
                        Nombre = Settings.UserName,
                        Id = Settings.userId,
                    };
                    //   MiDispositivo = new Dispositivo { DispositivoId = Settings.deviceId };
                    var main = MainViewModel.GetInstance();
                    main.LoadMenu(agenteView.Nombre);
                    NavigationService navigationService = new NavigationService();
                    navigationService.SetMainPage(agenteView);
                }
                else
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
            }
            else
            {
                MainPage = new NavigationPage(new NotInternetPage());
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}