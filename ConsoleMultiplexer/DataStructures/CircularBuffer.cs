using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ConsoleMultiplexer.DataStructures
{
	internal class CircularBuffer<T>
	{
		private readonly T[] _data;
		private readonly int _foldSize;

		private int _dataSize;
		private int _dataOffset;

		private int MaxSize => _data.Length;
		private bool IsFull => _dataSize == MaxSize;
		private int End => (_dataOffset + _dataSize) % MaxSize;

		public CircularBuffer(int size, int foldSize = 1)
		{
			_data = new T[size / foldSize * foldSize];
			_foldSize = foldSize;
		}

		public void Add(T value)
		{
			if (IsFull)
				MakeSpace();

			Push(value);
		}

		public void Clear()
		{
			_dataSize = 0;
		}

		private void MakeSpace()
		{
			_dataOffset = (_dataOffset + _foldSize) % MaxSize;
			_dataSize -= _foldSize;
		}

		private void Push(T value)
		{
			Debug.Assert(!IsFull);

			_data[End] = value;
			_dataSize++;
		}
	}
}
