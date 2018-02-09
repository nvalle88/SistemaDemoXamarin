using CityParkAgente.ViewModels;

namespace CityParkAgente.Infrastructure
{
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }
        public InstanceLocator()
        {
            Main = new MainViewModel();
        }
    }
}