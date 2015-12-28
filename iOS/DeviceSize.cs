using System;
using UIKit;

namespace BikeFinder.iOS
{
	public class DeviceSize
	{

		public int[] GetDimensions() {

			return new int[] {
				(int)UIScreen.MainScreen.Bounds.Width,
				(int)UIScreen.MainScreen.Bounds.Height
			};

		}
	}
}

