using System;
using System.Diagnostics;

namespace BikeFinder
{
	public static class DeviceDetails
	{
		public static double Width { get; set; }

		public static double Height { get; set; }

		public static double MapHeight { get { return 7 * Height / 9; } }

		public static double TopRowHeight { get { return Height / 9; } }
	}
}

