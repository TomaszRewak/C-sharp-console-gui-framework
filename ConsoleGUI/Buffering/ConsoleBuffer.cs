using ConsoleGUI.Data;
using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Buffering
{
	internal class ConsoleBuffer
	{
		private Cell?[,] _buffer = new Cell?[0, 0];

		public Size Size => new Size(_buffer.GetLength(0), _buffer.GetLength(1));

		public void Initialize(in Size size)
		{
			_buffer = new Cell?[size.Width, size.Height];
		}

		public bool Update(in Position position, in Cell newCell)
		{
			ref var cell = ref _buffer[position.X, position.Y];
			bool characterChanged = cell?.Character != newCell.Character;

			cell = newCell;

			return characterChanged;
		}

		public MouseContext? GetMouseContext(Position position)
		{
			if (!Size.Contains(position)) return null;

			return _buffer[position.X, position.Y]?.MouseListener;
		}
	}
}
