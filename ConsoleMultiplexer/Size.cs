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
		public static Size Of(int width, int height) => new Size(width, height);
		public static Size Containing(in Position position) => new Size(position.X + 1, position.Y + 1);
		public static Size Combine(in Size lhs, in Size rhs) => new Size(Math.Max(lhs.Width, rhs.Width), Math.Max(lhs.Height, rhs.Height));

		public bool Contains(in Size size)
		{
			return
				size.Width <= Width &&
				size.Height <= Height;
		}

		public bool Contains(in Position position)
		{
			return
				position.X < Width &&
				position.Y < Height;
		}

		public IEnumerator<Position> GetEnumerator()
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					yield return new Position(x, y);
		}
	}
}
