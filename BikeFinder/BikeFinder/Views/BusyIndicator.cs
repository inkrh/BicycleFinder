using System;
using Xamarin.Forms;

namespace BikeFinder
{
	public class BusyIndicator : ContentView
	{
		ActivityIndicator aIndicator;
		Label status;
		StackLayout slo;

		public string Status
		{
			get
			{
				if (status != null)
				{
					return status.Text;
				}
				return string.Empty;
			}
			set
			{
				if (status != null)
				{
					status.Text = value;
				}
			}
		}

		public BusyIndicator()
		{
			aIndicator = new ActivityIndicator
			{
				Color = Color.Red,
				IsRunning = true,
				IsVisible = true,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center
			};

			status = new Label
			{
				FontSize = 18,
				TextColor = Color.Red,
				IsVisible = true,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center
			};
			slo = new StackLayout
			{
				Children = { aIndicator, status },
				HeightRequest = DeviceDetails.Height,
				WidthRequest = DeviceDetails.Width,
				IsVisible = false,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center
			};

			Content = slo;
		}

		public bool Show
		{
			get
			{
				return slo.IsVisible;
			}

			set
			{
				if (string.IsNullOrEmpty(status.Text))
				{
					status.Text = "busy";
				}
				if (!value)
				{
					status.Text = string.Empty;
				}
				slo.IsVisible = value;
			}
		}
	}
}
