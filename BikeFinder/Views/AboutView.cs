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
				WidthRequest = DeviceDetails.Width - 80,
				Text = Constants.CityBikesURL,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			citybikes.Clicked += (object sender, EventArgs e) => openCityBikes ();
			title = new Label {
				WidthRequest = DeviceDetails.Width - 80,
				Text = Constants.Title,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			aboutText = new Label {
				WidthRequest = DeviceDetails.Width - 80,
				Text = Constants.AboutText,
				FontSize = 14,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			disclaimer = new Label {
				WidthRequest = DeviceDetails.Width - 80,
				Text = Constants.DisclaimerText,
				FontSize = 12,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
					
			};

			Grid.SetRow (title, 0);
			Grid.SetRow (aboutText, 1);
			Grid.SetRow (disclaimer, 2);
			Grid.SetRow (citybikes, 3);

			mainGrid = new Grid {
				HeightRequest = DeviceDetails.MapHeight + DeviceDetails.TopRowHeight,
				WidthRequest = DeviceDetails.Width,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				BackgroundColor = Color.White,
				RowDefinitions = new RowDefinitionCollection {
					new RowDefinition { Height = DeviceDetails.MapHeight / 4 },
					new RowDefinition { Height = DeviceDetails.MapHeight / 4 },
					new RowDefinition { Height = DeviceDetails.MapHeight / 4 },
					new RowDefinition { Height = DeviceDetails.MapHeight / 4 }
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

		void openCityBikes ()
		{
			MessagingCenter.Send ("websiteClick", "WEBSITE");
		}
	}
}


