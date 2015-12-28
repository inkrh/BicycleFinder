using System;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.Diagnostics;

namespace BikeFinder
{
	public class ExtendedSplash : ContentPage
	{
		Label status;
		Position currentPosition;
		Image icon;
		public ExtendedSplash ()
		{
			icon = new Image ();
			icon.Source = "Icon-60.png";
			icon.VerticalOptions = LayoutOptions.Center;
			icon.HorizontalOptions = LayoutOptions.Center;

			status = new Label ();
			status.VerticalOptions = LayoutOptions.End;
			status.HorizontalOptions = LayoutOptions.Center;

			Content = new StackLayout { 
				Children = {
					icon,status
				},
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(0,20,0,0)
					
					
			};
			runcall ();
		}

		async void runcall() {

			//try location twice
			status.Text = "trying location";
			if (await location ()) {
				status.Text = "location " + currentPosition.Latitude + ":" + currentPosition.Longitude;
			} else {
				status.Text = "location failed";
				if (await location ()) {
					status.Text = "location " + currentPosition.Latitude + ":" + currentPosition.Longitude;
				} 
			}

			LocationHandler.Instance.CurrentLocation = currentPosition;
			status.Text = "getting bike networks";
			var a = await NetworksController.Instance.GetNetworks ();

			if (a != null) {
				status.Text = "found " + a.Count + " cities";
			}
			Debug.WriteLine ("Going to next page");

			Application.Current.MainPage = new MapPage ();

		}

		async Task<bool> location() {
			Task<Position> p = LocationHandler.Instance.GetLocation ();

			currentPosition = await p;
			if (p.IsCanceled) {
				Debug.WriteLine ("Task Cancelled");
				currentPosition = new Position (0, 0);
				LocationHandler.Instance.LBS = false;
				return false;
			} else {
				return true;
			}

		}
	}
}


