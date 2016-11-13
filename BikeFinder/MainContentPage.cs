using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace BikeFinder
{
	public class MainContentPage : ContentPage
	{
		ButtonLabelView refreshLocation;
		ButtonLabelView aboutButton;
		ButtonLabelView altAboutButton;

		ButtonLabelView mainButton;
		ButtonLabelView cityListButton;
		ButtonLabelView routeButton;

		PinDetailCustomView pinDetailView;
		AboutView aboutView;

		City ChosenCity;

		StackLayout cityListStackLayout;
		Grid altSubMenuGrid;
		Grid subMenuGrid;
		Grid mainGrid;

		TableView cityTable;
		Map map;



		bool isConnected { get; set; }

		public MainContentPage ()
		{
			isConnected = true;
			pinDetailView = new PinDetailCustomView ();
			Grid.SetRow (pinDetailView, 0);
			pinDetailView.IsVisible = false;

			aboutView = new AboutView ();
			Grid.SetRow (aboutView, 0);
			Grid.SetRowSpan (aboutView, 2);
			aboutView.IsVisible = false;

			CrossConnectivity.Current.ConnectivityChanged += (sender, e) => {
				if (CrossConnectivity.Current.IsConnected != isConnected) {
					if (!CrossConnectivity.Current.IsConnected) {
						MessageBox (Constants.ConnectionLost, Constants.Error, Constants.Ok, null);
					} else
						Init ();
				}
				isConnected = CrossConnectivity.Current.IsConnected;
			};

			mainGrid = new Grid ();
			mainGrid.RowDefinitions = new RowDefinitionCollection {
				new RowDefinition{ Height = DeviceDetails.MapHeight },
				new RowDefinition{ Height = DeviceDetails.TopRowHeight },
				new RowDefinition{ Height = DeviceDetails.TopRowHeight },
			};
			mainGrid.RowSpacing = 0;
			mainButton = new ButtonLabelView (Constants.MainButton, Constants.Menu, new EventHandler ((sender, e) => showUI ()));

			refreshLocation = new ButtonLabelView (Constants.RefreshButton, Constants.Refresh, new EventHandler ((sender, e) => refresh ()));
			refreshLocation.VerticalOptions = LayoutOptions.Center;
			refreshLocation.HorizontalOptions = LayoutOptions.End;
			Grid.SetColumn (refreshLocation, 0);

			cityListButton = new ButtonLabelView (Constants.POI, Constants.Cities, new EventHandler ((sender, e) => showCityList ()));
			cityListButton.VerticalOptions = LayoutOptions.Center;
			cityListButton.HorizontalOptions = LayoutOptions.Center;
			Grid.SetColumn (cityListButton, 1);

			aboutButton = new ButtonLabelView (Constants.AboutButton, Constants.About, new EventHandler ((sender, e) => showAbout ()));
			aboutButton.VerticalOptions = LayoutOptions.Center;
			aboutButton.HorizontalOptions = LayoutOptions.Start;
			Grid.SetColumn (aboutButton, 2);

			cityTable = new TableView {
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				WidthRequest = DeviceDetails.Width - 80,
				HeightRequest = DeviceDetails.MapHeight - 40,
				IsVisible = false
			};
			PopulateTable ();

			subMenuGrid = new Grid {
				HeightRequest = DeviceDetails.TopRowHeight,
				WidthRequest = DeviceDetails.Width,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				BackgroundColor = Color.White,
				ColumnDefinitions = new ColumnDefinitionCollection {
					new ColumnDefinition{ Width = DeviceDetails.Width / 3 },
					new ColumnDefinition{ Width = DeviceDetails.Width / 3 },
					new ColumnDefinition{ Width = DeviceDetails.Width / 3 }
				},
				ColumnSpacing = 0,
				Children = { refreshLocation, cityListButton, aboutButton }
			};

			routeButton = new ButtonLabelView (Constants.RouteButton, Constants.Route, new EventHandler ((sender, e) => route ()));
			routeButton.VerticalOptions = LayoutOptions.Center;
			routeButton.HorizontalOptions = LayoutOptions.End;
			Grid.SetColumn (routeButton, 0);

			altAboutButton = new ButtonLabelView (Constants.AboutButton, Constants.About, new EventHandler ((sender, e) => showAbout ()));
			altAboutButton.VerticalOptions = LayoutOptions.Center;
			altAboutButton.HorizontalOptions = LayoutOptions.Start;
			Grid.SetColumn (altAboutButton, 1);

			altSubMenuGrid = new Grid {
				HeightRequest = DeviceDetails.TopRowHeight,
				WidthRequest = DeviceDetails.Width,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				BackgroundColor = Color.White,
				ColumnDefinitions = new ColumnDefinitionCollection {
					new ColumnDefinition{ Width = DeviceDetails.Width / 2 },
					new ColumnDefinition{ Width = DeviceDetails.Width / 2 }
				},
				Children = { routeButton, altAboutButton }
			};


			cityListStackLayout = new StackLayout {
				Padding = new Thickness (0, 40, 0, 0),
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Center,
				WidthRequest = DeviceDetails.Width,
				HeightRequest = DeviceDetails.MapHeight,
				Children = { cityTable },
				BackgroundColor = Color.White
			};


			cityListStackLayout.IsVisible = false;
			subMenuGrid.IsVisible = false;
			altSubMenuGrid.IsVisible = false;

			MapSpan initialSpan = new MapSpan (LocationHandler.Instance.CurrentLocation, 360, 360);
			map = new Map (initialSpan);

			Grid.SetRow (cityListStackLayout, 0);
			Grid.SetRowSpan (cityListStackLayout, 1);

			Grid.SetRow (subMenuGrid, 1);
			Grid.SetRowSpan (subMenuGrid, 1);

			Grid.SetRow (altSubMenuGrid, 1);
			Grid.SetRowSpan (altSubMenuGrid, 1);

			Grid.SetRow (map, 0);
			Grid.SetRowSpan (map, 2);

			Grid.SetRow (mainButton, 2);



			mainGrid.Children.Add (map);
			mainGrid.Children.Add (mainButton);

			mainGrid.Children.Add (cityListStackLayout);
			mainGrid.Children.Add (subMenuGrid);
			mainGrid.Children.Add (pinDetailView);
			mainGrid.Children.Add (aboutView);
			mainGrid.Children.Add (altSubMenuGrid);



			MessagingCenter.Subscribe<string, Network> (this, "CITY", (sender, args) => CityChosen (args as Network));
			MessagingCenter.Subscribe<string, City> (this, "PIN", (sender, args) => PinClicked (args as City));
			MessagingCenter.Subscribe<string> (this, "WEBSITE", (sender) => OpenWebsite ());

			Init ();
			Content = mainGrid;

		}

		void showAbout ()
		{
			aboutView.IsVisible = true;
			pinDetailView.IsVisible = false;
			subMenuGrid.IsVisible = false;
			altSubMenuGrid.IsVisible = false;
			cityListStackLayout.IsVisible = false;

		}

		void showUI ()
		{
			if (pinDetailView.IsVisible) {
				pinDetailView.IsVisible = false;
				altSubMenuGrid.IsVisible = false;
				return;
			}

			if (aboutView.IsVisible) {
				aboutView.IsVisible = false;
				return;
			}

			subMenuGrid.IsVisible = !subMenuGrid.IsVisible;
			cityListStackLayout.IsVisible = false;
		}

		void showCityList ()
		{
			cityListStackLayout.IsVisible = !cityListStackLayout.IsVisible;

		}

		async void Init ()
		{

			if (null == Networks.Instance.NetworkList) {
				await Networks.Instance.GetNetworks ();
			}

			await Networks.Instance.CalculateNetworkDistance ();
			CityChosen (Networks.Instance.ClosestNetwork);
			map.IsShowingUser = LocationHandler.Instance.LBS;
			PopulateTable ();

		}

		async void CityChosen (Network chosen)
		{
			cityListStackLayout.IsVisible = false;

			Networks.Instance.CurrentNetwork = chosen;

			if (!LocationHandler.Instance.LBS || Networks.Instance.CurrentNetwork != Networks.Instance.ClosestNetwork) {
				LocationHandler.Instance.CurrentLocation = new Position (Networks.Instance.CurrentNetwork.lat / 1E6,
					Networks.Instance.CurrentNetwork.lng / 1E6);
			}

			showMap (Networks.Instance.CurrentNetwork);

			cityListStackLayout.IsVisible = false;
			subMenuGrid.IsVisible = false;

		}

		void PinClicked (City city)
		{
			MapHandler.Instance.BackFromPinMapSpan = map.VisibleRegion;
			pinDetailView.ChosenNetwork = Networks.Instance.CurrentNetwork;
			ChosenCity = city;
			pinDetailView.ChosenCity = city;
			pinDetailView.IsVisible = true;
			altSubMenuGrid.IsVisible = true;
		}

		async public void BackFromPin (Network network)
		{
			CityChosen (network);
			map.MoveToRegion (MapHandler.Instance.BackFromPinMapSpan);
		}

		async void route ()
		{
			altSubMenuGrid.IsVisible = false;
			pinDetailView.IsVisible = false;
			Device.OpenUri (new Uri (string.Format (Constants.AppleMapsURL,
				WebUtility.UrlEncode (LocationHandler.Instance.CurrentLocation.Latitude.ToString ()),
				WebUtility.UrlEncode (LocationHandler.Instance.CurrentLocation.Longitude.ToString ()),
				WebUtility.UrlEncode ((ChosenCity.lat / 1E6).ToString ()),
				WebUtility.UrlEncode ((ChosenCity.lng / 1E6).ToString ()))));
		}

		async void showMap (Network chosen)
		{
			
			MapHandler.Instance.ClearPins (map);

			try {
				map.MoveToRegion (MapHandler.Instance.GetMap (chosen));

				var a = await CityController.Instance.GetCityData (chosen.url);
				Debug.WriteLine(chosen.url);
				if (a != null) {
					foreach (var c in Cities.Instance.CityData) {
						MapHandler.Instance.DropPin (map, c.lat / 1E6, c.lng / 1E6, c);
						Debug.WriteLine("Placing " + c.name);
					}
				}

				map.MoveToRegion (MapHandler.Instance.CalculateBoundingCoordinates (chosen, map));

			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
			}



		}

		async void refresh ()
		{
			await LocationHandler.Instance.GetLocation ();

			Init ();

			subMenuGrid.IsVisible = false;
			cityListStackLayout.IsVisible = false;
		}

		void PopulateTable ()
		{
			if (Networks.Instance.NetworkList != null && Networks.Instance.NetworkList.Count > 0) {
				cityTable.IsVisible = true;
				cityTable.Intent = TableIntent.Data;
				cityTable.Root = ResultsTableRoot ();
				cityTable.HasUnevenRows = true;
			} else {
				cityTable.IsVisible = false;
			}
		}

		TableRoot ResultsTableRoot ()
		{
			var temp = new TableRoot ();
			var tempSection = new TableSection ();
			foreach (var item in Networks.Instance.NetworkList) {
				tempSection.Add (new NetworkTableCell (item));
			}
			temp.Add (tempSection);
			return temp;
		}


		async Task<string> MessageBox (string text, string title, string option1, string option2)
		{
			bool answer = true;
			if (null == option2) {
				await DisplayAlert (title, text, option1);
			} else {
				answer = await DisplayAlert (title, text, option1, option2);
			}

			if (answer) {
				return option1;
			} else {
				return option2;
			}
		}

		async void OpenWebsite ()
		{
			pinDetailView.IsVisible = false;
			altSubMenuGrid.IsVisible = false;
			cityListStackLayout.IsVisible = false;
			subMenuGrid.IsVisible = false;
			aboutView.IsVisible = false;
			Device.OpenUri (new Uri (Constants.CityBikesURL));
		}

	}
}


