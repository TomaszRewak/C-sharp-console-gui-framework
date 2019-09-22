using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Data
{
	public struct Color
	{
		public byte Red { get; set; }
		public byte Green { get; set; }
		public byte Blue { get; set; }

		public Color(byte red, byte green, byte blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
		}

		public static Color White => new Color(255, 255, 255);
		public static Color Black => new Color(0, 0, 0);
		public static Color LightBlue => new Color(100, 100, 255);
		public static Color Gray => new Color(100, 100, 100);
	}
}
