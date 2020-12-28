using Xamarin;
using System;
using Foundation;
using UIKit;

namespace BikeFinder.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			FormsMaps.Init ();


			LoadApplication (new App ());

			DeviceDetails.Width = (double) UIScreen.MainScreen.Bounds.Width;
			DeviceDetails.Height = (double) UIScreen.MainScreen.Bounds.Height;

			return base.FinishedLaunching (app, options);
		}
	}
}

