namespace CityParkAgente.Interfaces
{
    public interface INetworkConnection
    {
        bool IsConnected { get; set; }
        void CheckNetworkConnection();
    }
}
