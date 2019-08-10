using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	internal struct CursorPosition
	{
		int X { get; }
		int Y { get; }

		public CursorPosition(int x, int y, int spaceWidth)
		{
			X = x;
			Y = y;
		}
	}
}
