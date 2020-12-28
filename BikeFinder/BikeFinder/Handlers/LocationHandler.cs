using System.Threading.Tasks;
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

		public Position CurrentLocation;

		public bool LBS { get; set; }

		public async Task<bool> GetLocation ()
		{
			await Networks.Instance.GetNetworks();
			if (null != Networks.Instance.CurrentNetwork)
			{
				CurrentLocation = new Position(Networks.Instance.CurrentNetwork.lat / 1E6, Networks.Instance.CurrentNetwork.lng / 1E6);
			}
			LBS = null!=Networks.Instance.CurrentNetwork;
			
			return LBS;
		}

	}
}

