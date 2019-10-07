using ConsoleMultiplexer.Common;
using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Space;
using ConsoleMultiplexer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class Grid : Control
	{
		struct ColumnDefinition
		{
			int Width;
		}

		struct RowDefinition
		{
			int Height;
		}

		private int _columns;
		public int Columns
		{
			get => _columns;
			set => Setter
				.Set(ref _columns, value);
		}

		private int _rows;
		public int Rows
		{
			get => _rows;
			set => Setter
				.Set(ref _rows, value);
		}

		public override Character this[Position position] => throw new NotImplementedException();

		protected override void Initialize()
		{
			throw new NotImplementedException();
		}
	}
}
