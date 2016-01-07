using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using System;

namespace BikeFinder
{
	public class LocationHandler
	{
		static LocationHandler instance;

		LocationHandler ()
		{
		}

		public static LocationHandler Instance {
			get {
				if (instance == null) {
					instance = new LocationHandler ();
				}
				return instance;
			}
		}

		public Position CurrentLocation;

		public bool LBS { get; set; }

		public async Task<bool> GetLocation ()
		{
			try {
			CrossGeolocator.Current.DesiredAccuracy = 50;
			if (!CrossGeolocator.Current.IsGeolocationAvailable || !CrossGeolocator.Current.IsGeolocationEnabled) {
				LBS = false;
			} else {
				var location = await CrossGeolocator.Current.GetPositionAsync (5000);
				CurrentLocation = new Position (location.Latitude, location.Longitude);
				LBS = true;
			}
			} catch (Exception e) {
				LBS = false;
				Debug.WriteLine (e.Message);
			}
			return LBS;
		}

	}
}

