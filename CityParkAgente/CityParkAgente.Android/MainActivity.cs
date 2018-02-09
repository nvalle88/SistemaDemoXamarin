using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Permissions;
using Xamarin;

namespace CityParkAgente.Droid
{
    [Activity(Label = "CityParkAgente", Icon = "@drawable/ic_City_Park", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            FormsMaps.Init(this, bundle);
            

            LoadApplication(new App());
        }
      
    }
}