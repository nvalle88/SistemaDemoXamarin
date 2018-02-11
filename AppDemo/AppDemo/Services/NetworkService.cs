using AppDemo.Interfaces;
using Xamarin.Forms;

namespace AppDemo.Services
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