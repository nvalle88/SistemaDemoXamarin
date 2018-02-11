using System.Threading.Tasks;
/// <summary>
/// Nos ayuda a crear mensajes dentro de la aplicación 
/// </summary>
namespace AppDemo.Services
{
    public class DialogService
    {
        public async Task ShowMessage(string title, string message)
        {
            await App.Current.MainPage.DisplayAlert(title, message, "Aceptar");
        }
    }
}