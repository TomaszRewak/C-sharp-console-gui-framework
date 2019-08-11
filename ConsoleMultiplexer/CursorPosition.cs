using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	internal struct CursorPosition
	{
		private int DistanceFromBeginning { get; }
		public int ScreenWidth { get; }

		public int X => DistanceFromBeginning % ScreenWidth;
		public int Y => DistanceFromBeginning / ScreenWidth;

		public bool IsLineBeginning => X == 0;

		public CursorPosition(int distanceFromBeginning, int screenWidth)
		{
			DistanceFromBeginning = distanceFromBeginning;
			ScreenWidth = screenWidth;
		}

		public CursorPosition Move(int by)
		{
			return new CursorPosition(
				DistanceFromBeginning + by,
				ScreenWidth);
		}

		public CursorPosition Next()
		{
			return Move(1);
		}

		public CursorPosition NextLine()
		{
			return Move(ScreenWidth - X);
		}

		public static int operator-(in CursorPosition lhs, in CursorPosition rhs)
		{
			return rhs.DistanceFromBeginning - lhs.DistanceFromBeginning;
		}
	}
}
