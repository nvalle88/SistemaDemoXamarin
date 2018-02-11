using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppDemo.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PonerMultaPage : ContentPage
    {
        static PonerMultaPage instance;
        public Plugin.Geolocator.Abstractions.Position Location;

        public static PonerMultaPage GetInstance()
        {
            if (instance == null)
            {
                instance = new PonerMultaPage();
            }

            return instance;
        }
        public PonerMultaPage()
        {
            InitializeComponent();
            instance = this;
            Location = new Plugin.Geolocator.Abstractions.Position();
            Locator();
        }

        async void Locator()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            Location = await locator.GetPositionAsync(timeoutMilliseconds: 5000);
        }
    }
}