using System.Collections.Generic;

namespace BikeFinder
{
	public class Networks
	{
		private static Networks instance;

		private Networks ()
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

