using CityParkAgente.Classes;
using CityParkAgente.Helpers;
using System;
using System.Threading.Tasks;
/// <summary>
/// SignalR nos ayuda a mantener el realtime en mensajes pero lo utilizamos para enviar nuestra posicion actual
/// </summary>
namespace CityParkAgente.Services
{
    public class SignalRService
    {
        public static SignalRClient SignalRClient = new SignalRClient(Constants.Constants.CityparkWeb);
        DialogService dialogService = new DialogService();
        /// <summary>
        /// esta tarea permite enviar la posicion segun los parametros de latitud y longitud
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public async Task SendPosition(float lat, float lon)
        {
            await SignalRClient.Start().ContinueWith(task =>
                 {
                     if (task.IsFaulted)
                         dialogService.ShowMessage("Error", "An error occurred when trying to connect to SignalR: " + task.Exception.InnerExceptions[0].Message);
                 }
                   );
            LivePositionRequest lpr = new LivePositionRequest
            {
                EmpresaId = Settings.companyId,
                AgenteId = App.AgenteActual.AgenteId,
                Nombre = App.AgenteActual.Nombre + " " + App.AgenteActual.Apellido,
                fecha = DateTime.Now,
                Lat = lat,
                Lon = lon
            };
            SignalRClient.SendMessage(lpr);
        }
    }
}