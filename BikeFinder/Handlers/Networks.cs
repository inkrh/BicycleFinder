using System.Collections.Generic;

namespace BikeFinder
{
	public class Networks
	{
		static Networks instance;

		Networks ()
		{
		}

		public static Networks Instance {
			get {
				if (instance == null) {
					instance = new Networks ();
				}
				return instance;
			}
		}

		public List<Network> NetworkList { get; set; }

		public Network CurrentNetwork { get; set; }

	}
}

