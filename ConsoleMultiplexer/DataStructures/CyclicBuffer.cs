using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.DataStructures
{
	internal class CyclicBuffer<T>
	{
		private readonly T[] _buffer;
		private int _dataSize;

		public int FoldSize { get; set; }
		private bool IsFull => _dataSize == _buffer.Length;

		public CyclicBuffer(int size)
		{

		}

		public void Add()
		{
			if (IsFull)
		}

		private void MakeSpace()
		{

		}
	}
}
