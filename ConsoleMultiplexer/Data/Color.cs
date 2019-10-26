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

		public Color Mix(in Color color, float factor) => this * (1 - factor) + color * factor;

		public static Color White => new Color(255, 255, 255);
		public static Color Black => new Color(0, 0, 0);

		public static bool operator ==(in Color lhs, in Color rhs)
		{
			return
				lhs.Red == rhs.Red &&
				lhs.Green == rhs.Green &&
				lhs.Blue == rhs.Blue;
		}

		public static bool operator !=(in Color lhs, in Color rhs) => !(lhs == rhs);

		public static Color operator *(in Color color, float factor) => new Color((byte)(color.Red * factor), (byte)(color.Green * factor), (byte)(color.Blue * factor));
		public static Color operator +(in Color lhs, in Color rhs) => new Color(
			(byte)Math.Min(byte.MaxValue, lhs.Red + rhs.Red), 
			(byte)Math.Min(byte.MaxValue, lhs.Green + rhs.Green),
			(byte)Math.Min(byte.MaxValue, lhs.Blue + rhs.Blue));

		public override bool Equals(object obj)
		{
			return obj is Color color && this == color;
		}

		public override int GetHashCode()
		{
			var hashCode = -1058441243;
			hashCode = hashCode * -1521134295 + Red.GetHashCode();
			hashCode = hashCode * -1521134295 + Green.GetHashCode();
			hashCode = hashCode * -1521134295 + Blue.GetHashCode();
			return hashCode;
		}
	}
}
