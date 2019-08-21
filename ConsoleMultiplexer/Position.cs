using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public struct Position
	{
		public int X { get; }
		public int Y { get; }

		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static Position Begin => new Position(0, 0);

		public Position Next => new Position(X + 1, Y);
		public Position NextLine => new Position(0, Y + 1);
	}
}
