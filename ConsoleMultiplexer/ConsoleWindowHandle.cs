using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public class ConsoleWindowHandle
	{
		private readonly int _left;
		private readonly int _top;
		private readonly int _width;
		private readonly int _height;

		public ConsoleWindowHandle(int left, int top, int width, int height)
		{
			_left = left;
			_top = top;
			_width = width;
			_height = height;
		}

		public void WriteLine(string text)
		{
			Console.MoveBufferArea(_left, _top + 1, _width, _height - 1, _left, _top);
			Console.SetCursorPosition(_left, _top + _height - 1);
			for (int i = 0; i < _width; i++)
				Console.Write(' ');
			Console.SetCursorPosition(_left, _top + _height - 1);
			Console.Write(text);
		}
	}
}
