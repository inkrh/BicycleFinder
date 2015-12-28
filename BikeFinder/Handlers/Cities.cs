using System.Collections.Generic;

namespace BikeFinder
{
	public class Cities
	{
		private static Cities instance;

		private Cities ()
		{
		}

		public static Cities Instance {
			get {
				if (instance == null) {
					instance = new Cities ();
				}
				return instance;
			}
		}

		public List<City> CityData { get; set; }

		public City CurrentCity { get; set; }

	}
}

