using System;

using Xamarin.Forms;

namespace BikeFinder
{
	public class PinDetailCustomView : ContentView
	{
		Grid mainGrid = new Grid ();
		Label title;

		//network details
		Label networkTitle;
		Label networkDetails;

		//city details
		Label cityTitle;
		Label cityDetails;

		Network chosenNetwork;

		public Network ChosenNetwork {
			get { return chosenNetwork; } 
			set { 
				chosenNetwork = value;
				if (null != networkTitle) {
					networkTitle.Text = value.city;
				}
				if (null != networkDetails) {
					networkDetails.Text = (value.lat / 1E6).ToString () + " : " + (value.lng / 1E6).ToString ();
				}
			}
		}

		City chosenCity;

		public City ChosenCity {
			get { return chosenCity; }
			set {
				chosenCity = value;
				if (null != cityTitle) {
					cityTitle.Text = value.name;
				}
				if (null != cityDetails) {
					cityDetails.Text = value.bikes + " available\n" + value.free + " free slots\n" +
					(value.lat / 1E6).ToString () + " : " + (value.lng / 1E6).ToString ();
				}
			}
		}

		public PinDetailCustomView ()
		{

			title = new Label {
				WidthRequest = DeviceDetails.Width - 80,
				Text = "Bike Station Details",
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				XAlign = TextAlignment.Center
			};

			Grid.SetRow (title, 0);


			networkTitle = new Label {
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,

			};

			networkDetails = new Label {
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,

			};

			var networkSLO = new StackLayout {
				Orientation = StackOrientation.Vertical,
				HeightRequest = (DeviceDetails.Height / 2) - 90,
				WidthRequest = (DeviceDetails.Width) - 90,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				Children = { networkTitle, networkDetails },
			};

			Grid.SetRow (networkSLO, 1);

			cityTitle = new Label {
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,

			};

			cityDetails = new Label {
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,

			};

			var citySLO = new StackLayout {
				Orientation = StackOrientation.Vertical,
				HeightRequest = (DeviceDetails.Height / 2) - 90,
				WidthRequest = (DeviceDetails.Width) - 90,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				Children = { cityTitle, cityDetails }
			};

			Grid.SetRow (citySLO, 2);

			mainGrid = new Grid {
				HeightRequest = DeviceDetails.MapHeight,
				WidthRequest = DeviceDetails.Width,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				BackgroundColor = Color.White,
				RowDefinitions = new RowDefinitionCollection {
					new RowDefinition { Height = DeviceDetails.MapHeight /3 },
					new RowDefinition { Height = DeviceDetails.MapHeight /3 },
					new RowDefinition { Height = DeviceDetails.MapHeight /3 }
				},
				Children = {
					title,
					networkSLO,
					citySLO
				}
			};
				

			Content = mainGrid;

		}
	}
}


