﻿using System;
using System.Collections.Generic;
using System.Linq;
using XLabs.Forms;
using Foundation;
using UIKit;
using ProgressRingControl.Forms.Plugin.iOS;

namespace AppDemo.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
 
            global::Xamarin.Forms.Forms.Init();
            var imageButtonRenderer = new XLabs.Forms.Controls.ImageButtonRenderer();
            var isr = new XLabs.Forms.Controls.ImageSourceConverter();
            var igr = new XLabs.Forms.Controls.ImageGalleryRenderer();
            var pgr = new ProgressRingRenderer();
            ButtonCircle.FormsPlugin.iOS.ButtonCircleRenderer.Init();

            LoadApplication(new App());
            ProgressRingRenderer.Init();
            Xamarin.FormsMaps.Init();           
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            return base.FinishedLaunching(app, options);
        }
    }
}
