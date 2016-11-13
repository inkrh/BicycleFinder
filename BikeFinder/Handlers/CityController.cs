using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BikeFinder
{
	public class CityController
	{
		static CityController instance;

		CityController ()
		{
		}

		public static CityController Instance {
			get {
				if (instance == null) {
					instance = new CityController ();
				}
				return instance;
			}
		}

		public async Task<List<City>> GetCityData (string url)
		{
			var result = await GetResponse (url);
			if (result.StatusCode != HttpStatusCode.OK)
				return null;


			Cities.Instance.CityData = Deserialize (result.Content.ReadAsStringAsync ().Result);
			foreach (var i in Cities.Instance.CityData)
			{
				Debug.WriteLine(i.name);
			}
			return Cities.Instance.CityData;
		}


		static async Task<HttpResponseMessage> GetResponse (string url)
		{
			var httpClient = new HttpClient ();
			HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Get, url);
			var response = await httpClient.SendAsync (request);
			Debug.WriteLine(response);
			return response;
		}

		static List<City> Deserialize (string jsonString)
		{
			return JsonConvert.DeserializeObject<List<City>> (jsonString);
		}


	}
}

