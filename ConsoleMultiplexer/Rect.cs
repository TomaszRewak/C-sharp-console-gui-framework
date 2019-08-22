using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public struct Rect
	{
		public int Left { get; }
		public int Top { get; }
		public int Width { get; }
		public int Height { get; }

		public Rect(int left, int top, int width, int height)
		{
			Left = left;
			Top = top;
			Width = width;
			Height = height;
		}
	}
}
