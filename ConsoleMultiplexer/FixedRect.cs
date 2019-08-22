using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	struct FixedRect
	{
		int Width { get; }
		int Height { get; }

		public FixedRect(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public static Size Of(int width, int height) => new Size(width, height);

		public bool Contains(in Position position)
		{
			return
				position.X >= 0 &&
				position.Y >= 0 &&
				position.X < Width &&
				position.Y < Height;
		}
	}
}
