using ConsoleGUI.Data;
using System;

namespace ConsoleGUI.Utils
{
	internal static class ColorConverter
	{
		public static ConsoleColor GetNearestConsoleColor(Color color)
		{
			if (Math.Max(Math.Max(color.Red, color.Green), color.Blue) - Math.Min(Math.Min(color.Red, color.Green), color.Blue) < 32)
			{
				int brightness = ((int)color.Red + (int)color.Green + (int)color.Blue) / 3;
				if (brightness < 64) return ConsoleColor.Black;
				if (brightness < 160) return ConsoleColor.DarkGray;
				if (brightness < 224) return ConsoleColor.Gray;
				return ConsoleColor.White;
			}
			int index = (color.Red > 128 | color.Green > 128 | color.Blue > 128) ? 8 : 0;
			index |= (color.Red > 64) ? 4 : 0;
			index |= (color.Green > 64) ? 2 : 0;
			index |= (color.Blue > 64) ? 1 : 0;
			return (ConsoleColor)index;
		}

		public static Color GetColor(ConsoleColor color)
		{
			if (color == ConsoleColor.DarkGray) return new Color(128, 128, 128);
			if (color == ConsoleColor.Gray) return new Color(192, 192, 192);
			int index = (int)color;
			byte d = ((index & 8) != 0) ? (byte)255 : (byte)128;
			return new Color(
				((index & 4) != 0) ? d : (byte)0,
				((index & 2) != 0) ? d : (byte)0,
				((index & 1) != 0) ? d : (byte)0);
		}
	}
}
