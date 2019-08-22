using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.DataStructures
{
	internal class Buffer2D<T>
	{
		private T[,] _buffer;
		private Size _bufferSize;

		public Size Size { get; }

		public T Get(in Position position)
		{
			if (!Size.Contains(position)) throw new IndexOutOfRangeException(nameof(position));

			return _buffer[position.X, position.Y];
		}

		public void Set(in Position position, in T value)
		{
			if ()
		}

		private struct Cell
		{
			T Value;
			bool Changed;
		}
	}
}
