using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Data
{
	public readonly struct Color
	{
		public byte Red { get; }
		public byte Green { get; }
		public byte Blue { get; }

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

		public static bool operator ==(in Color lhs, in Color rhs)
		{
			return
				lhs.Red == rhs.Red &&
				lhs.Green == rhs.Green &&
				lhs.Blue == rhs.Blue;
		}

		public static bool operator !=(in Color lhs, in Color rhs) => !(lhs == rhs);

		public override bool Equals(object obj)
		{
			return obj is Color color && this == color;
		}
	}
}
