using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	internal struct CursorPosition
	{
		private int DistanceFromBeginning { get; }
		private int BufferWidth { get; }

		public int X => DistanceFromBeginning % BufferWidth;
		public int Y => DistanceFromBeginning / BufferWidth;

		public bool IsLineBeginning => X == 0;

		public CursorPosition(int distanceFromBeginning, int screenWidth)
		{
			DistanceFromBeginning = distanceFromBeginning;
			BufferWidth = screenWidth;
		}

		public CursorPosition Move(int by)
		{
			return new CursorPosition(
				DistanceFromBeginning + by,
				BufferWidth);
		}

		public CursorPosition Next()
		{
			return Move(1);
		}

		public CursorPosition NextLine()
		{
			return Move(BufferWidth - X);
		}

		public int GetBufferWidth()
		{
			return BufferWidth;
		}

		public static int operator-(in CursorPosition lhs, in CursorPosition rhs)
		{
			return rhs.DistanceFromBeginning - lhs.DistanceFromBeginning;
		}
	}
}
