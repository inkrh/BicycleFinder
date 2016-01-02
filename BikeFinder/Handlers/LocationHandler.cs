using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;

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


		Plugin.Geolocator.Abstractions.IGeolocator locator;

		public Position CurrentLocation;

		bool lbs;

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

