using Android.App;
using Android.Content.PM;
using Android.OS;

namespace AppDemo.Droid
{
    [Activity(Theme = "@style/Theme.Splash",
       MainLauncher = true,
       NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class PantallaInicio : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            System.Threading.Thread.Sleep(2000);
            StartActivity(typeof(MainActivity));

            // Create your application here
        }
    }
}