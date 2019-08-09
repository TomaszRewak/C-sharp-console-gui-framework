using System;

namespace ConsoleMultiplexer
{
	[Flags]
	public enum WindowBorder
	{
		None   = 0b0000,
		Left   = 0b0001,
		Top    = 0b0010,
		Right  = 0b0100,
		Bottom = 0b1000,
		All    = 0b1111
	}

	internal static class WindowBorderExtension
	{
		internal static int CountBorders(this WindowBorder border, WindowBorder type)
		{
			int borders = 0;
			for (var bordersToCheck = (int)(border & type); bordersToCheck > 0; bordersToCheck >>= 1)
				borders += bordersToCheck & 1;
			return borders;
		}
	}
}
