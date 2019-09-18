using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public struct Size
	{
		public int Width { get; }
		public int Height { get; }

		public Size(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public static Size Empty => new Size(0, 0);
		public static Size Containing(in Position position) => new Size(position.X + 1, position.Y + 1);
		public static Size Max(in Size lhs, in Size rhs) => new Size(Math.Max(lhs.Width, rhs.Width), Math.Max(lhs.Height, rhs.Height));
		public static Size Min(in Size lhs, in Size rhs) => new Size(Math.Min(lhs.Width, rhs.Width), Math.Min(lhs.Height, rhs.Height));
		public static Size Clip(in Size min, in Size value, in Size max) => Size.Max(min, Size.Min(max, value));
		public static Size Of(Array array) => new Size(array.GetLength(0), array.GetLength(1));

		public bool Contains(in Size size)
		{
			return
				size.Width <= Width &&
				size.Height <= Height;
		}

		public bool Contains(in Position position)
		{
			return
				position.X >= 0 &&
				position.Y >= 0 &&
				position.X < Width &&
				position.Y < Height;
		}

		public Rect AsRect() => new Rect(0, 0, Width, Height);

		public Size Expand(int width, int height) => new Size(Width + width, Height + height);
		public Size Shrink(int width, int height) => new Size(Width - width, Height - height);
		public Size WithHeight(int height) => new Size(Width, height);
		public Size WithWidth(int width) => new Size(width, Height);

		public Size WithInfitineHeight => new Size(Width, int.MaxValue);
		public Size WithInfitineWidth => new Size(int.MaxValue, Height);

		public IEnumerator<Position> GetEnumerator()
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					yield return new Position(x, y);
		}

		public static bool operator ==(in Size lhs, in Size rhs) => lhs.Equals(rhs);
		public static bool operator !=(in Size lhs, in Size rhs) => !(lhs == rhs);
		public static bool operator <=(in Size lhs, in Size rhs) => lhs.Width <= rhs.Width && lhs.Height <= rhs.Height;
		public static bool operator >=(in Size lhs, in Size rhs) => lhs.Width >= rhs.Width && lhs.Height >= rhs.Height;

		public override string ToString()
		{
			return $"({Width}, {Height})";
		}
	}
}
