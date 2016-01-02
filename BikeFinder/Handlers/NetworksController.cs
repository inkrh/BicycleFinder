using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BikeFinder
{
	public class NetworksController
	{
		static NetworksController instance;

		NetworksController ()
		{
		}

		public static NetworksController Instance {
			get {
				if (instance == null) {
					instance = new NetworksController ();
				}
				return instance;
			}
		}



		public async Task<List<Network>> GetNetworks ()
		{
			try {
				var result = await GetResponse ("http://api.citybik.es/networks.json");
				if (result.StatusCode != HttpStatusCode.OK) {
					Debug.WriteLine ("Failed: " + result.StatusCode.ToString ());
					return null;
				}
				Networks.Instance.NetworkList = Deserialize (result.Content.ReadAsStringAsync ().Result);
				Networks.Instance.NetworkList.Sort ((x, y) => x.city.CompareTo (y.city));
				return Networks.Instance.NetworkList;
			} catch (Exception e) {
				Debug.WriteLine ("GetNetworks() Exception : " + e.Message);
			}
			return null;
		}

		static async Task<HttpResponseMessage> GetResponse (string url)
		{
			try {
				var httpClient = new HttpClient ();
				HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Get, url);
				var response = await httpClient.SendAsync (request);
				return response;
			} catch (Exception e) {
				Debug.WriteLine (e.Message);
				return null;
			}
		}

		static List<Network> Deserialize (string jsonString)
		{
			return JsonConvert.DeserializeObject<List<Network>> (jsonString);
		}
	}
}

