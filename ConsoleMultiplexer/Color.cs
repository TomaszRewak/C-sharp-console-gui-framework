using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
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
	}
}
