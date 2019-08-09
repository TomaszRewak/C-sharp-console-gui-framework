using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public struct ScreenRect
	{
		public int Left { get; }
		public int Top { get; }
		public int Width { get; }
		public int Height { get; }

		public ScreenRect(int left, int top, int width, int height)
		{
			Left = left;
			Top = top;
			Width = width;
			Height = height;
		}

		internal ScreenRect Crop(WindowBorder border)
		{
			return new ScreenRect(
				Left   + border.CountBorders(WindowBorder.Left),
				Top    + border.CountBorders(WindowBorder.Top),
				Width  - border.CountBorders(WindowBorder.Left | WindowBorder.Right),
				Height - border.CountBorders(WindowBorder.Top | WindowBorder.Bottom));
		}
	}
}
