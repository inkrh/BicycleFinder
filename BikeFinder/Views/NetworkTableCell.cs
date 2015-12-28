using System;
using Xamarin.Forms;

namespace BikeFinder
{
	public class NetworkTableCell : ViewCell
	{
		Network network;

		Label cityName;
		StackLayout mainStack;

		public NetworkTableCell (Network chosen)
		{
			network = chosen;
			cityName = new Label {
				Text = network.city,
				VerticalOptions = LayoutOptions.Center
			};
			mainStack = new StackLayout {
				Children = { cityName },
				VerticalOptions = LayoutOptions.Center
			};
			Tapped += cellTapped;
			View = mainStack;
			
		}

		void cellTapped (object sender, EventArgs e)
		{
			if (network != null) {
				MessagingCenter.Send ("cityCell", "CITY", network);
			}
		}
	}


}



