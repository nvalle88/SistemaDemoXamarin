using AppDemo.Pages;
using AppDemo.ViewModels;
using System.Threading.Tasks;
/// <summary>
/// En esta clase se encuentran los metodos para navegar dentro de la aplicación de una pagina a otra
/// </summary>
namespace AppDemo.Services
{
    public class NavigationService
    {
        public void NavigateBack() => App.Navigator.PopAsync();

        public async Task Navigate(string pageName)
        {
            App.Master.IsPresented = false;
            switch (pageName)
            {
                case "VerificarAutoPage":
                    await App.Navigator.PushAsync(new VerificarAutoPage(), true);
                    break;

                case "ConsultarMultas":
                    await App.Navigator.PushAsync(new ConsultarAutoPage());
                    break;
                case "PonerMulta":
                    await App.Navigator.PushAsync(new PonerMultaPage(), true);
                    break;

                case "PasswordPage":
                    await App.Navigator.PushAsync(new PasswordPage());
                    break;

                //case "MetodoPago":
                //    await App.Navigator.PushAsync(new MetodoPago());
                //    break;

                //case "TarjetasPage":
                //    await App.Navigator.PushAsync(new TarjetasCreditosPage());
                //    break;

                //case "PrepagoPage":
                //    await App.Navigator.PushAsync(new MetodoPago());
                //    break;

                //case "NuevaTarjetaPage":
                //    await App.Navigator.PushAsync(new NuevaTarjetaCreditoPage());
                //    break;

                //case "NuevoParqueoPage":
                //    await App.Navigator.PushAsync(new NuevoParqueoPage());
                //    break;

                //case "NuevoCarroPage":
                //    await App.Navigator.PushAsync(new NuevoCarroPage());
                //    break;

                //case "ComprarSaldoPage":
                //    await App.Navigator.PushAsync(new ComprarSaldoPage());
                //    break;


                //case "PromocionesPage":
                //    await App.Navigator.PushAsync(new PromocionesPage());
                //    break;

                case "MainPage":
                    await App.Navigator.PopToRootAsync();
                    break;

                default: break;
            }
        }

        internal void SetMainPage(AgenteViewModel agenteActual)
        {
            var main = MainViewModel.GetInstance();
            App.AgenteActual = agenteActual;
            main.CargarLugares();
            App.Current.MainPage = new MasterPage();
        }

        public AgenteViewModel GetAgenteActual() => App.AgenteActual;
    }
}