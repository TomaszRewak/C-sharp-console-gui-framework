using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	internal struct CursorPosition
	{
		private int DistanceFromBeginning { get; }
		private int ScreenWidth { get; }

		public int X => DistanceFromBeginning % ScreenWidth;
		public int Y => DistanceFromBeginning / ScreenWidth;

		public bool IsLineBeginning => X == 0;

		public CursorPosition(int distanceFromBeginning, int screenWidth)
		{
			DistanceFromBeginning = distanceFromBeginning;
			ScreenWidth = screenWidth;
		}

		public CursorPosition NextLine()
		{
			return new CursorPosition(
				DistanceFromBeginning + ScreenWidth - X,
				ScreenWidth);
		}
	}
}
