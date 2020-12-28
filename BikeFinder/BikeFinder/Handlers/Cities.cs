using System.Collections.Generic;

namespace BikeFinder
{
	public class Cities
	{
		static Cities instance;

		Cities ()
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

