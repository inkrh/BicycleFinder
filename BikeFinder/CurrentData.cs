using System;
using Xamarin.Forms.Maps;

namespace BikeFinder
{
	public class CurrentData
	{
		private static CurrentData instance;

		private CurrentData ()
		{
		}

		public static CurrentData Instance {
			get {
				if (instance == null) {
					instance = new CurrentData ();
				}
				return instance;
			}
		}

		public Network CurrentNetwork {get; set;}

		public City CurrentCity {get;set;}

		public Position CurrentPosition {get;set;}



	}
}

