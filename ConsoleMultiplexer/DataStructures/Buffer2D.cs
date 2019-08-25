using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.DataStructures
{
	internal class Buffer2D<T>
	{
		private Cell[,] _buffer = new Cell[0, 0];
		private List<Position> _changes = new List<Position>();
		private Size _bufferSize = Size.Empty;

		public Size Size { get; private set; }
		public IEnumerable<Position> Changes => _changes;

		public T Get(in Position position)
		{
			if (!Size.Contains(position)) throw new IndexOutOfRangeException(nameof(position));

			return _buffer[position.X, position.Y].Value;
		}

		public void Set(in Position position, in T value)
		{
			AdjustBufferSize(position);

			At(position).Value = value;

			AdjustSize(position);
			MarkAsChanged(position);
		}

		public void ClearChanges()
		{
			_changes.Clear();
		}

		public void Clear()
		{
			ClearChanges();
			Size = Size.Empty;
		}

		private ref Cell At(Position position)
		{
			return ref _buffer[position.X, position.Y];
		}

		private void AdjustSize(in Position newPosition)
		{
			Size = Size.Union(Size, Size.Containing(newPosition));
		}

		private void AdjustBufferSize(in Position position)
		{
			if (_bufferSize.Contains(position)) return;

			var newBufferSize = Size.Of(
				_bufferSize.Width < Size.Width ? Size.Width * 2 : _bufferSize.Width,
				_bufferSize.Height < Size.Height ? Size.Height * 2 : _bufferSize.Height);
			var newBuffer = new Cell[_bufferSize.Width, _bufferSize.Height];

			foreach (var p in Size)
				newBuffer[p.X, p.Y] = _buffer[p.X, p.Y];

			_buffer = newBuffer;
			_bufferSize = newBufferSize;
		}

		private void MarkAsChanged(in Position position)
		{
			ref var cell = ref At(position);

			if (cell.Changed) return;

			cell.Changed = true;
			_changes.Add(position);
		}

		private struct Cell
		{
			public T Value;
			public bool Changed;
		}
	}
}
