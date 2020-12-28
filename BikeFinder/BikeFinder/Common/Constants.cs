namespace BikeFinder
{
	public static class Constants
	{

		public const string NetworkURL = "https://api.citybik.es/networks.json";
		public static string AppleMapsURL = "https://maps.apple.com/?daddr={0},{1}";
		public const string CityBikesURL = "https://citybik.es";
		public const string MainIcon = "Icon-60.png";
		public const string MainButton = "MainButton.png";
		public const string RefreshButton = "Refresh.png";
		public const string POI = "POI.png";
		public const string AboutButton = "About.png";
		public const string RouteButton = "Route.png";

		public const string Route = "route";
		public const string Menu = "menu";
		public const string Refresh = "refresh";
		public const string TryingLocation = "trying location";
		public const string Cities = "cities";
		public const string About = "about";

		public const string LocationFailed = "location failed";

		public const string TryingNetworks = "getting bike networks";

		public const string Found = "found ";

		public const string CitiesSpace = " cities";

		public const string Location = "location ";
		public const string ConnectionLost = "Data connection lost";
		public const string Error = "Error";
		public const string Ok = "Ok";
		public const string PinAvailable = "Available : ";
		public const string Available = " available\n";
		public const string Free = " free slots\n";
		public const string BikeStation = "Bike Station Details";
		public const string Title = "BicycleFinder";
		public const string AboutText = "BicycleFinder provides information on public bike stations around the world. \n\nThe user's location is used solely to show the nearest bike stations when available.";
		public const string DisclaimerText = "BicycleFinder is an independently created app which uses information from the citybik.es api. Although every effort has been made to ensure reliability, bike station details and the number of available bikes cannot be guaranteed.";
	}
}

