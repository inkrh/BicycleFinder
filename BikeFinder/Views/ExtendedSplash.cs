using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

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
					icon, status
				},
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness (0, 20, 0, 0)
			};

			runcall ();
		}

		async void runcall ()
		{
			//try location twice (bug on some devices/OS versions causes first location attempt to fail)
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
			var networksResult = await NetworksController.Instance.GetNetworks ();

			if (networksResult != null) {
				status.Text = "found " + networksResult.Count + " cities";
			}

			Application.Current.MainPage = new MapPage ();

		}

		async Task<bool> location ()
		{
			var locationResult = LocationHandler.Instance.GetLocation ();
			//pushing it out for other methods(later enhancements planned)
			currentPosition = await locationResult;

			if (locationResult.IsCanceled) {
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


