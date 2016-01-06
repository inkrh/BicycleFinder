using System.Threading.Tasks;
using Xamarin.Forms;

namespace BikeFinder
{
	public class ExtendedSplash : ContentPage
	{
		Label status;
		Image icon;

		public ExtendedSplash ()
		{
			icon = new Image ();
			icon.Source = Constants.MainIcon;
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

			tryLocation ();
		}

		async void tryLocation ()
		{
			//try location twice (bug on some devices/OS versions causes first location attempt to fail)
			status.Text = Constants.TryingLocation;
			if (await LocationHandler.Instance.GetLocation ()) {
				updateLocationStatus ();
			} else {
				status.Text = Constants.LocationFailed;
				if (await LocationHandler.Instance.GetLocation ()) {
					updateLocationStatus ();
				} 
			}

			status.Text = Constants.TryingNetworks;


			if (await Networks.Instance.GetNetworks ()) {
				status.Text = Constants.Found + Networks.Instance.NetworkList.Count + Constants.CitiesSpace;
			}

			Application.Current.MainPage = new MainContentPage ();

		}

		void updateLocationStatus ()
		{
			status.Text = Constants.Location + LocationHandler.Instance.CurrentLocation.Latitude + ":" + LocationHandler.Instance.CurrentLocation.Longitude;
		}

	}
}


