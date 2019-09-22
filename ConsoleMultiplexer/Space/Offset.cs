using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Space
{
	public struct Offset
	{
		public int Left { get; }
		public int Top { get; }
		public int Right { get; }
		public int Bottom { get; }

		public Offset(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}
	}
}
