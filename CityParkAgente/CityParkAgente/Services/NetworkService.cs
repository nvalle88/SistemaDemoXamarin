using CityParkAgente.Interfaces;
using Xamarin.Forms;

namespace CityParkAgente.Services
{
    public class NetworkService
    {
        public bool IsConected()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            return networkConnection.IsConnected;
        }
    }
}