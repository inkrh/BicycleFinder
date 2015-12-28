using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace BikeFinder
{
	public class MapHandler
	{

		private static MapHandler instance;

		private MapHandler ()
		{
		}

		public static MapHandler Instance {
			get {
				if(instance == null) {
					instance = new MapHandler ();
					
				}
				return instance;
			}
		}

		public MapSpan CurrentMapSpan { get; set; } 

		public MapSpan BackFromPinMapSpan {get;set;}

		public MapSpan GetMap(Network chosen) {
			
			try{
				var span = MapSpan.FromCenterAndRadius (new Position (chosen.lat/1E6, 
					chosen.lng/1E6), 
					Distance.FromMeters (chosen.radius));
				span = span.WithZoom(10);
				CurrentMapSpan = span;
				return span;
			} catch (Exception ex) {
				return null;
			}
		}

		public Dictionary<Pin, City> PinCityDictionary;

		public void DropPin(Map map, double lat, double lon, City city) {
			if (PinCityDictionary == null) {
				PinCityDictionary = new Dictionary<Pin, City> ();
			}

			var position = new Position(lat,lon); // Latitude, Longitude
			var pin = new Pin {
				Type = PinType.Place,
				Position = position,
				Label = city.name,
				Address = "Available : " + city.bikes
			};

			if (map != null) {
				map.Pins.Add (pin);
				if (PinCityDictionary.ContainsKey (pin)) {
					PinCityDictionary [pin] = city;
				} else {
					PinCityDictionary.Add (pin, city);
				}
			}
			pin.Clicked += MapTo;
		}

		void MapTo(object sender, EventArgs e) {
			MessagingCenter.Send<string,City> ("mapPin", "PIN", PinCityDictionary [sender as Pin]);
		}

		public double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'C')
		{
			double rlat1 = Math.PI*lat1/180;
			double rlat2 = Math.PI*lat2/180;
			double theta = lon1 - lon2;
			double rtheta = Math.PI*theta/180;
			double dist = 
				Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * 
				Math.Cos(rlat2) * Math.Cos(rtheta);
			dist = Math.Acos(dist);
			dist = dist*180/Math.PI;
			dist = dist*60*1.1515;

			switch (unit)
			{
			case 'K': //Kilometers
				return dist * 1.609344;
			case 'N': //Nautical Miles 
				return dist * 0.8684;
			case 'M': //Miles
				return dist;
			case 'C': //Custom
				return dist * 0.75;
			}

			return dist;
		}

		public Dictionary<Pin, double> PinDistanceDictionary;

		public Pin ClosestPin { get; set; }

		public MapSpan CalculateBoundingCoordinates (Network chosen, Map thismap) {
			var region = thismap.VisibleRegion;
			if (region == null) {
				region = MapSpan.FromCenterAndRadius (new Position (chosen.lat, chosen.lng), Distance.FromMeters(chosen.radius));
			}

			var center = region.Center;
			var halfheightDegrees = region.LatitudeDegrees / 2;
			var halfwidthDegrees = region.LongitudeDegrees / 2;

			var left = center.Longitude - halfwidthDegrees;
			var right = center.Longitude + halfwidthDegrees;
			var top = center.Latitude + halfheightDegrees;
			var bottom = center.Latitude - halfheightDegrees;

			if (left < -180) left = 180 + (180 + left);
			if (right > 180) right = (right - 180) - 180;



			if (LocationHandler.Instance.CurrentLocation == new Position (0,0)) {
				LocationHandler.Instance.CurrentLocation = new Position (chosen.lat, chosen.lng);
				LocationHandler.Instance.LBS = false;

			}
			if (LocationHandler.Instance.LBS) {
				var l1 = LocationHandler.Instance.CurrentLocation.Latitude;
				var lo1 = LocationHandler.Instance.CurrentLocation.Longitude;

				PinDistanceDictionary = new Dictionary<Pin,double> ();

				foreach (var i in thismap.Pins) {
					var l2 = i.Position.Latitude;
					var lo2 = i.Position.Longitude;
					var td = MapHandler.Instance.DistanceTo (l1, lo1, l2, lo2);
					if (PinDistanceDictionary.ContainsKey (i)) {
						PinDistanceDictionary [i] = td;
					} else {
						PinDistanceDictionary.Add (i, td);
					}
					Debug.WriteLine (i.Label);
				}
				var closest = PinDistanceDictionary.OrderBy (x => x.Value).First ().Key;
				ClosestPin = closest;
		
				var redoCenter = new Position ((closest.Position.Latitude + l1) / 2, (closest.Position.Longitude + lo1) / 2);
				var redoMapSpan = MapSpan.FromCenterAndRadius (redoCenter, 
					Distance.FromMiles (PinDistanceDictionary [closest]*1.3));
				return redoMapSpan ;

			} else {
				return MapSpan.FromCenterAndRadius (LocationHandler.Instance.CurrentLocation, Distance.FromMeters (chosen.radius/6)); 
				}
		}

	}
}

