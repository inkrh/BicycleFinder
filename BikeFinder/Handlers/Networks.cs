using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using System.Linq;

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

		public Dictionary<double, Network> DistanceNetworks { get; set; }

		public Network ClosestNetwork {
			get { 
				if (DistanceNetworks != null) {
					var distance = DistanceNetworks.Keys.ToList ();
					distance.Sort ();
					return DistanceNetworks [distance [0]];
				}
				return null;
			}
		}

		public List<Network> NetworkList { get; set; }

		public Network CurrentNetwork { get; set; }

		public async Task<bool> GetNetworks ()
		{
			NetworkList = await NetworksController.Instance.GetNetworks ();
			return NetworkList.Count >= 1;
		}

		public async Task<bool> CalculateNetworkDistance ()
		{
			if (null == DistanceNetworks) {
				DistanceNetworks = new Dictionary<double,Network> ();
			}
			try{
			foreach (var network in NetworkList) {
				var n = Math.Abs (((network.lat / 1E6) - LocationHandler.Instance.CurrentLocation.Latitude) + ((network.lng / 1E6) - LocationHandler.Instance.CurrentLocation.Longitude));
				if (DistanceNetworks.ContainsKey (n)) {
					DistanceNetworks [n] = network;
				} else {
					DistanceNetworks.Add (n, network);
				}
			}
				return true;
			} catch {
				return false;
			}
		}

	}
}

