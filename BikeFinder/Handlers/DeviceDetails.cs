using System;
using System.Diagnostics;

namespace BikeFinder
{
	public static class DeviceDetails
	{
		private static double width;
		private static double height;

		public static double Width {
			get { return width; }
			set {
				Debug.WriteLine ("Width now : " + value);
				width = value;
			}
		}

		public static double Height {
			get { return height; }
			set {
				Debug.WriteLine ("Height now : " + value); 
				height = value;
			}
		}

		public static double MapHeight { get { return 7 * Height / 9; } }

		public static double TopRowHeight { get { return Height / 9; } }
	}
}

