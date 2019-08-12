using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	internal class BufferControl : IControl
	{
		private readonly Character[] _buffer;

		private int _dataOffset;
		private int _dataSize;

		private CursorPosition _start;
		public CursorPosition Start
		{
			get => _start;
			set
			{
				if (_start.X != value.X)
					ClearBuffer();

				_start = value;
			}
		}

		public CursorPosition End => Start.Move(_dataSize);

		private bool IsFull => _dataSize == _buffer.Length;

		public BufferControl(int bufferLength)
		{
			_buffer = new Character[bufferLength];
		}

		private void ClearBuffer()
		{
			_dataSize = 0;
			_dataOffset = 0;
		}

		private void FoldBuffer()
		{
			_dataOffset = (_dataOffset + Start.GetBufferWidth()) % _buffer.Length;
			_dataSize -= Start.GetBufferWidth();
		}

		public void Add(in Character character)
		{
			if (character.IsNewLine)
				AddNewLine();
			else
				Push(character);
		}

		public void AddNewLine()
		{
			do Push(Character.Empty);
			while (!End.IsLineBeginning);
		}

		private void Push(in Character character)
		{
			if (IsFull)
				FoldBuffer();

			_buffer[(_dataOffset + _dataSize) % _buffer.Length] = character;
		}

		public Character Get(in CursorPosition cursorPosition)
		{
			return _buffer[(cursorPosition - Start + _dataOffset) % _buffer.Length];
		}
	}
}
