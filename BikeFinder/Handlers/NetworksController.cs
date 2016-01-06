using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

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
				var result = await GetResponse (Constants.NetworkURL);
				Debug.WriteLine(result.StatusCode);
				if (result.StatusCode != HttpStatusCode.OK) {
					return null;
				}
				var list = Deserialize (result.Content.ReadAsStringAsync ().Result);
				list.Sort ((x, y) => x.city.CompareTo (y.city));
				return list;
			} catch {
				return null;
			}
		}

		static async Task<HttpResponseMessage> GetResponse (string url)
		{
			try {
				var httpClient = new HttpClient ();
				var request = new HttpRequestMessage (HttpMethod.Get, url);
				var response = await httpClient.SendAsync (request);
				return response;
			} catch {
				return null;
			}
		}

		static List<Network> Deserialize (string jsonString)
		{
			return JsonConvert.DeserializeObject<List<Network>> (jsonString);
		}
	}
}

