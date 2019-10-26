using ConsoleGUI.Data;
using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Buffering
{
	internal class ConsoleBuffer
	{
		private Character?[,] buffer;

		public void Initialize(in Size size)
		{
			buffer = new Character?[size.Width, size.Height];
		}

		public bool Update(in Position position, in Character character)
		{
			ref var cell = ref buffer[position.X, position.Y];

			if (cell == character) return false;

			cell = character;

			return true;
		}
	}
}
