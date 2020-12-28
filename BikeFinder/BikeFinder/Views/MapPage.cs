using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace BikeFinder
{
	public class MapPage : ContentPage
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
		BusyIndicator aIndicator;

		const double motionDelta = 1.5;

		bool isConnected { get; set; }

		public MapPage ()
		{
			isConnected = true;
			pinDetailView = new PinDetailCustomView ();
			Grid.SetRow (pinDetailView, 0);
			pinDetailView.IsVisible = false;

			aboutView = new AboutView ();
			Grid.SetRow (aboutView, 0);
			Grid.SetRowSpan (aboutView, 2);
			aboutView.IsVisible = false;

			CrossDeviceMotion.Current.SensorValueChanged += Shaken;
			CrossDeviceMotion.Current.Start (MotionSensorType.Accelerometer, MotionSensorDelay.Default);
			CrossConnectivity.Current.ConnectivityChanged += (object sender, ConnectivityChangedEventArgs e) => {
				if (CrossConnectivity.Current.IsConnected != isConnected) {
					if (!CrossConnectivity.Current.IsConnected) {
						//show connection lost message
						MessageBox ("Data connection lost", "Connectivity", "Ok", null);
					} else
						runCall ();
				}
				isConnected = CrossConnectivity.Current.IsConnected;
			};
			aIndicator = new BusyIndicator ();
			aIndicator.HorizontalOptions = LayoutOptions.Center;
			aIndicator.VerticalOptions = LayoutOptions.Center;
			aIndicator.Show = false;

			mainGrid = new Grid ();
			mainGrid.RowDefinitions = new RowDefinitionCollection {
				new RowDefinition{ Height = DeviceDetails.MapHeight },
				new RowDefinition{ Height = DeviceDetails.TopRowHeight },
				new RowDefinition{ Height = DeviceDetails.TopRowHeight },
			};
			mainGrid.RowSpacing = 0;
			mainButton = new ButtonLabelView ("MainButton.png", "menu", new EventHandler ((sender, e) => showUI ()));

			refreshLocation = new ButtonLabelView ("Refresh.png", "refresh", new EventHandler ((sender, e) => refresh ()));
			refreshLocation.VerticalOptions = LayoutOptions.Center;
			refreshLocation.HorizontalOptions = LayoutOptions.End;
			Grid.SetColumn (refreshLocation, 0);

			cityListButton = new ButtonLabelView ("POI.png", "cities", new EventHandler ((sender, e) => showCityList ()));
			cityListButton.VerticalOptions = LayoutOptions.Center;
			cityListButton.HorizontalOptions = LayoutOptions.Center;
			Grid.SetColumn (cityListButton, 1);

			aboutButton = new ButtonLabelView ("About.png", "about", new EventHandler ((sender, e) => showAbout ()));
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

			routeButton = new ButtonLabelView ("Route.png", "route", new EventHandler ((sender, e) => route ()));
			routeButton.VerticalOptions = LayoutOptions.Center;
			routeButton.HorizontalOptions = LayoutOptions.End;
			Grid.SetColumn (routeButton, 0);

			altAboutButton = new ButtonLabelView ("About.png", "about", new EventHandler ((sender, e) => showAbout ()));
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

			Grid.SetRow (aIndicator, 0);
			Grid.SetRowSpan (aIndicator, 3);

			mainGrid.Children.Add (map);
			mainGrid.Children.Add (mainButton);

			mainGrid.Children.Add (cityListStackLayout);
			mainGrid.Children.Add (subMenuGrid);
			mainGrid.Children.Add (pinDetailView);
			mainGrid.Children.Add (aboutView);
			mainGrid.Children.Add (altSubMenuGrid);

			aIndicator.HeightRequest = DeviceDetails.Width / 3;
			aIndicator.WidthRequest = DeviceDetails.Width / 3;
			mainGrid.Children.Add (aIndicator);

			Content = mainGrid;

			MessagingCenter.Subscribe<string, Network> (this, "CITY", (sender, args) => CityChosen (args as Network));
			MessagingCenter.Subscribe<string, City> (this, "PIN", (sender, args) => PinClicked (args as City));
			MessagingCenter.Subscribe<string> (this, "WEBSITE", (sender) => OpenWebsite ());

			runCall ();

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

		async void runCall ()
		{
			if (!isConnected)
				return;
			
			aIndicator.Show = true;

			if (Networks.Instance.NetworkList != null) {
				if (LocationHandler.Instance.CurrentLocation != new Position (0, 0) && LocationHandler.Instance.LBS) {
					var networkDictionary = new Dictionary<double,Network> ();
					foreach (var i in Networks.Instance.NetworkList) {
						var n = Math.Abs (((i.lat / 1E6) - LocationHandler.Instance.CurrentLocation.Latitude) + ((i.lng / 1E6) - LocationHandler.Instance.CurrentLocation.Longitude));
						if (networkDictionary.ContainsKey (n)) {
							networkDictionary [n] = i;
						} else {
							networkDictionary.Add (n, i);
						}
					}
					List<double> sortedDistance = new List<double> ();

					foreach (var i in networkDictionary) {
						sortedDistance.Add (i.Key);
					}

					sortedDistance.Sort ();
					Networks.Instance.CurrentNetwork = networkDictionary [sortedDistance [0]];
					PopulateTable ();
					showMap (Networks.Instance.CurrentNetwork);
					map.IsShowingUser = true;
					aIndicator.Show = false;

				} else {
					LocationHandler.Instance.LBS = false;
					PopulateTable ();
					aIndicator.Show = false;
					map.IsShowingUser = false;
				}
			}
		}

		async void CityChosen (Network chosen)
		{
			
			
			aIndicator.Show = true;

			if (cityListStackLayout.IsVisible) {
				cityListStackLayout.IsVisible = false;
			}
			Networks.Instance.CurrentNetwork = chosen;

			if (Networks.Instance.NetworkList == null || Networks.Instance.NetworkList.Count < 1)
				return;


			LocationHandler.Instance.CurrentLocation = new Position (Networks.Instance.CurrentNetwork.lat / 1E6,
				Networks.Instance.CurrentNetwork.lng / 1E6);
			showMap (Networks.Instance.CurrentNetwork);
			cityListStackLayout.IsVisible = false;
			subMenuGrid.IsVisible = false;
			aIndicator.Show = false;

		}

		void PinClicked (City city)
		{
			CrossDeviceMotion.Current.Stop (MotionSensorType.Accelerometer);
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
			Device.OpenUri (new Uri (string.Format ("http://maps.apple.com/?saddr={0},{1}&daddr={2},{3}",
				WebUtility.UrlEncode (LocationHandler.Instance.CurrentLocation.Latitude.ToString ()),
				WebUtility.UrlEncode (LocationHandler.Instance.CurrentLocation.Longitude.ToString ()),
				WebUtility.UrlEncode ((ChosenCity.lat / 1E6).ToString ()),
				WebUtility.UrlEncode ((ChosenCity.lng / 1E6).ToString ()))));
		}

		async void showMap (Network chosen)
		{
			if (!isConnected)
				return;
			MapHandler.Instance.ClearPins (map);
			try {
				map.MoveToRegion (MapHandler.Instance.GetMap (chosen));

				var a = await CityController.Instance.GetCityData (chosen.url);
				if (a != null) {
					aIndicator.Show = true;
					foreach (var c in Cities.Instance.CityData) {
						MapHandler.Instance.DropPin (map, c.lat / 1E6, c.lng / 1E6, c);
					}
					aIndicator.Show = false;
				}

				map.MoveToRegion (MapHandler.Instance.CalculateBoundingCoordinates (chosen, map));

			} catch {

			}



		}

		bool refreshing { get; set; }

		async void Shaken (object s, SensorValueChangedEventArgs a)
		{
			if (refreshing)
				return;
			
			if (Math.Abs (((MotionVector)a.Value).X) > motionDelta || Math.Abs (((MotionVector)a.Value).Y) > motionDelta || Math.Abs (((MotionVector)a.Value).Z) > motionDelta) {
				Debug.WriteLine ("**** Shaken - " + Math.Max (Math.Max (Math.Abs (((MotionVector)a.Value).X), Math.Abs (((MotionVector)a.Value).Y)), Math.Abs (((MotionVector)a.Value).Z)));
				Debug.WriteLine ("refresh triggered");
				refresh ();
			}



		}

		async void refresh ()
		{
			if (!isConnected)
				return;
			
			refreshing = aIndicator.Show = true;

			await LocationHandler.Instance.GetLocation ();

			runCall ();

			subMenuGrid.IsVisible = false;

			cityListStackLayout.IsVisible = false;

			refreshing = aIndicator.Show = false;

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
			aIndicator.Show = false;
			aboutView.IsVisible = false;
			Device.OpenUri (new Uri ("http://citybik.es"));
		}
	}
}


