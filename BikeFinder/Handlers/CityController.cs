using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace BikeFinder
{
	public class CityController
	{
		private static CityController instance;

		private CityController(){}

		public static CityController Instance
		{
			get{
				if (instance == null) {
					instance = new CityController ();
				}
				return instance;
			}
		}

		public async Task<List<City>> GetCityData(string url) {
			var r = await GetResponse(url);
			if (r.StatusCode != HttpStatusCode.OK) {
				Debug.WriteLine ("Failed: " + r.StatusCode.ToString ());
				return null;
			}
			Cities.Instance.CityData = Deserialize (r.Content.ReadAsStringAsync ().Result);
			return Cities.Instance.CityData;
		}


		static async Task<HttpResponseMessage> GetResponse(string url) 
		{
			var httpClient = new HttpClient();
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
			var response = await httpClient.SendAsync(request);
			return response;
		}

		static List<City> Deserialize(string a) {

			var n = JsonConvert.DeserializeObject<List<City>>(a);
			return n;

		}


	}
}

