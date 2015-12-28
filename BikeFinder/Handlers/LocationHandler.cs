using System;
using Geolocator.Plugin;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using System.Threading;
using System.Diagnostics;

namespace BikeFinder
{
	public class LocationHandler
	{
		private static LocationHandler instance;

		private LocationHandler ()
		{
		}

		Geolocator.Plugin.Abstractions.IGeolocator locator;

		public Position CurrentLocation;
		private bool lbs;

		public bool LBS {
			get { 
				if (!locator.IsGeolocationEnabled || !locator.IsGeolocationAvailable) {
					lbs = false;
				}
				return lbs;
			}
			set {
				lbs = value;
			}
		}

		public static LocationHandler Instance {
			get {
				if (instance == null) {
					instance = new LocationHandler ();
				}
				return instance;
			}
		}

		public async Task<Position> GetLocation ()
		{
			try {
				CurrentLocation = await getLocation ();
				LBS = true;
			} catch (Exception e) {
				Debug.WriteLine (e.Message);
				CurrentLocation = new Position (0, 0);
				LBS = false;
			}
			return CurrentLocation;
		}

		async Task<Position> getLocation ()
		{
			
			locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;
			if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled) {
				
				var position = await locator.GetPositionAsync (5000);
				if (position != null) {
					LBS = true;
					return new Position (position.Latitude, position.Longitude);
				} else {
					LBS = false;
					return new Position (0, 0);
				}
			} else {
				LBS = false;
				return new Position (0, 0);

			}
		}
	}
}

