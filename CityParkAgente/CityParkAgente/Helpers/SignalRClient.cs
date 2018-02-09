using CityParkAgente.Classes;
using Microsoft.AspNet.SignalR.Client;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CityParkAgente.Helpers
{
    public class SignalRClient : INotifyPropertyChanged
    {
        HubConnection Connection;
        IHubProxy ChatHubProxy;

        public delegate void MessageReceived(string username, string message);
        public event MessageReceived OnMessageReceived;

        public SignalRClient(string url)
        {
            Connection = new HubConnection(url);

            Connection.StateChanged += (StateChange obj) =>
            {
                OnPropertyChanged("ConnectionState");
            };

            ChatHubProxy = Connection.CreateHubProxy("recived");
            ChatHubProxy.On<string, string>("MessageReceived", (username, text) =>
            {
                OnMessageReceived?.Invoke(username, text);
            });
        }

        public void SendMessage(LivePositionRequest livePosition)
        {
            ChatHubProxy.Invoke("SendPosition", livePosition);
        }

        public Task Start() => Connection.Start();

        public bool IsConnectedOrConnecting => Connection.State != ConnectionState.Disconnected;

        public ConnectionState ConnectionState => Connection.State;

        public static async Task<SignalRClient> CreateAndStart(string url)
        {
            var client = new SignalRClient(url);
            await client.Start();
            return client;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}