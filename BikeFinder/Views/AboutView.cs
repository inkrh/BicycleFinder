using System;

using Xamarin.Forms;

namespace BikeFinder
{
	public class AboutView : ContentView
	{
		Grid mainGrid;
		Label title;
		Label aboutText;
		Label disclaimer;
		Button citybikes;

		public AboutView ()
		{
			citybikes = new Button {
				WidthRequest = DeviceDetails.Width-80,
				Text = "http://citybik.es",
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			citybikes.Clicked += (object sender, EventArgs e) => openCityBikes ();
			title = new Label {
				WidthRequest = DeviceDetails.Width - 80,
				Text = "BicycleFinder",
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			aboutText = new Label {
				WidthRequest = DeviceDetails.Width - 80,
				Text = "BicycleFinder provides information on public bike stations around the world. \n\nIt uses the user's location if available to show the nearest bike stations. If no location information is available, or if there is no city bike network near you, BicycleFinder still allows you to choose from available cities in the city bikes network.\n\nUsers can find routes to their chosen bike station using the iOS map feature.",

//					"BicycleFinder provides information on public bike stations around the world. " +
//				"The application uses the user's location if available to show the nearest bike stations " +
//				"or if not allows the user choose from a selection of available cities." +
//				"Users can find the routes to their chosen bike station using platform specific maps and routing.",
				FontSize = 14,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			disclaimer = new Label {
				WidthRequest = DeviceDetails.Width - 80,
				Text= "BicycleFinder is an independently created app which uses information from the citybik.es api. Although every effort has been made to ensure reliability, bike station details and the number of available bikes cannot be guaranteed.",
				FontSize = 12,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
					
			};
			Grid.SetRow (title, 0);
			Grid.SetRow (aboutText, 1);
			Grid.SetRow (disclaimer, 2);
			Grid.SetRow (citybikes, 3);

			mainGrid = new Grid {
				HeightRequest = DeviceDetails.MapHeight+DeviceDetails.TopRowHeight,
				WidthRequest = DeviceDetails.Width,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				BackgroundColor = Color.White,
				RowDefinitions = new RowDefinitionCollection {
					new RowDefinition { Height = DeviceDetails.MapHeight /4 },
					new RowDefinition { Height = DeviceDetails.MapHeight /4 },
					new RowDefinition { Height = DeviceDetails.MapHeight /4 },
					new RowDefinition { Height = DeviceDetails.MapHeight /4 }
				},
				Children = {
					title,
					aboutText,
					disclaimer,
					citybikes
				}
			};


			Content = mainGrid;
		}

		async void openCityBikes ()
		{
			MessagingCenter.Send ("websiteClick", "WEBSITE");

		}
	}
}


