using ConsoleGUI.Data;
using System;

namespace ConsoleGUI.Utils
{
	internal static class ColorConverter
	{
		public static ConsoleColor GetNearestConsoleColor(Color color)
		{
			int index = (color.Red > 128 | color.Green > 128 | color.Blue > 128) ? 8 : 0;
			index |= (color.Red > 64) ? 4 : 0;
			index |= (color.Green > 64) ? 2 : 0;
			index |= (color.Blue > 64) ? 1 : 0;
			return (ConsoleColor)index;
		}
	}
}
