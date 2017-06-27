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

		static MapHandler instance;

		MapHandler ()
		{
		}

		public static MapHandler Instance {
			get {
				if (instance == null) {
					instance = new MapHandler ();
					
				}
				return instance;
			}
		}

		public MapSpan CurrentMapSpan { get; set; }

		public MapSpan BackFromPinMapSpan { get; set; }

		public MapSpan GetMap (Network chosen)
		{
			
			try {
                
				var span = MapSpan.FromCenterAndRadius (new Position (chosen.lat / 1E6, 
					           chosen.lng / 1E6), 
					           Distance.FromMeters (chosen.radius));
				span = span.WithZoom (10);
				CurrentMapSpan = span;
				return span;
			} catch (Exception ex) {
				return null;
			}
		}

		public Dictionary<Pin, City> PinCityDictionary;

		public void ClearPins (Map map)
		{
			map.Pins.Clear ();
			PinCityDictionary = new Dictionary<Pin, City> ();
		}

		public void DropPin (Map map, double lat, double lon, City city)
		{
			if (PinCityDictionary == null) {
				PinCityDictionary = new Dictionary<Pin, City> ();
			}

			var position = new Position (lat, lon); // Latitude, Longitude
			var pin = new Pin {
				Type = PinType.Place,
				Position = position,
				Label = city.name,
				Address = Constants.PinAvailable + city.bikes
			};

			city.Distance = Math.Abs (((city.lat / 1E6) - LocationHandler.Instance.CurrentLocation.Latitude) + ((city.lng / 1E6) - LocationHandler.Instance.CurrentLocation.Longitude));

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


		void MapTo (object sender, EventArgs e)
		{
			MessagingCenter.Send<string,City> ("mapPin", "PIN", PinCityDictionary [sender as Pin]);
		}

		public Pin ClosestPin { get; set; }

		public MapSpan CalculateBoundingCoordinates (Network chosen, Map thismap)
		{

			//expand outwards to 2nd closest pin
			if (PinCityDictionary.Count > 1)
			{
				ClosestPin = PinCityDictionary.OrderBy(x => x.Value.Distance).ToList()[1].Key;
			}
			else {
				ClosestPin = PinCityDictionary.OrderBy(x => x.Value.Distance).First().Key;
			}
		
			var redoCenter = new Position ((ClosestPin.Position.Latitude + LocationHandler.Instance.CurrentLocation.Latitude) / 2, 
				                 (ClosestPin.Position.Longitude + LocationHandler.Instance.CurrentLocation.Longitude) / 2);

			var redoRadius = distance (ClosestPin.Position, LocationHandler.Instance.CurrentLocation);

			var redoMapSpan = MapSpan.FromCenterAndRadius (redoCenter, redoRadius);
			 

			return redoMapSpan;

		}


		//from http://www.geodatasource.com
		private Distance distance (Position p1, Position p2)
		{
			double lat1 = p1.Latitude;
			double lon1 = p1.Longitude;
			double lat2 = p2.Latitude;
			double lon2 = p2.Longitude;

			double theta = lon1 - lon2;
			double dist = Math.Sin (deg2rad (lat1)) * Math.Sin (deg2rad (lat2)) + Math.Cos (deg2rad (lat1)) * Math.Cos (deg2rad (lat2)) * Math.Cos (deg2rad (theta));
			dist = Math.Acos (dist);
			dist = rad2deg (dist);
			dist = dist * 60 * 1.1515;
			dist = dist * 0.8684;
			//weighting for optimal display
			dist = dist /1.6;
			return Distance.FromMiles(dist);
		}

		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		//::  This function converts decimal degrees to radians             :::
		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		private double deg2rad (double deg)
		{
			return (deg * Math.PI / 180.0);
		}

		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		//::  This function converts radians to decimal degrees             :::
		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		private double rad2deg (double rad)
		{
			return (rad / Math.PI * 180.0);
		}

	}
}

