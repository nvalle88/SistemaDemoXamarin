using AppDemo.ViewModels;

namespace AppDemo.Infrastructure
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