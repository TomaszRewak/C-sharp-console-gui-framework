using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGUI.Controls
{
	public class DataGrid<T> : Control
	{
		public class ColumnDefinition
		{
			private readonly Func<int, Character> _headerSelector;
			private readonly Func<T, int, Character> _valueSelector;

			public readonly int Width;

			public ColumnDefinition(string header, int width, Func<T, string> selector)
			{
				Width = width;

				_headerSelector = i =>
				{
					var text = header;
					return i < text.Length ? new Character(text[i]) : Character.Empty;
				};

				_valueSelector = (v, i) =>
				{
					var text = selector(v);
					return i < text.Length ? new Character(text[i]) : Character.Empty;
				};
			}

			public ColumnDefinition(string header, int width, Func<T, int, Character> selector)
			{
				Width = width;

				_headerSelector = i =>
				{
					var text = header;
					return i < text.Length ? new Character(text[i]) : Character.Empty;
				};

				_valueSelector = selector;
			}

			public ColumnDefinition(int width, Func<int, Character> headerSelector, Func<T, int, Character> valueSelector)
			{
				Width = width;

				_headerSelector = headerSelector;
				_valueSelector = valueSelector;
			}

			public ColumnDefinition(
				string header,
				int width,
				Func<T, string> selector,
				Func<T, Color?> foreground = null,
				Func<T, Color?> background = null,
				TextAlignment textAlignment = TextAlignment.Left)
			{
				Width = width;

				_headerSelector = i =>
				{
					var text = header;
					return i < text.Length ? new Character(text[i]) : Character.Empty;
				};

				_valueSelector = (v, i) =>
				{
					var text = selector(v);

					if (textAlignment == TextAlignment.Center)
						i -= (Width - text.Length) / 2;
					if (textAlignment == TextAlignment.Right)
						i -= Width - text.Length;

					return new Character(
						i >= 0 && i < text.Length ? (char?)text[i] : null,
						foreground?.Invoke(v),
						background?.Invoke(v));
				};
			}

			internal Character GetHeader(int xOffset) => _headerSelector(xOffset);
			internal Character GetValue(T value, int xOffset) => _valueSelector(value, xOffset);
		}

		private ColumnDefinition[] _columns = new ColumnDefinition[0];
		public ColumnDefinition[] Columns
		{
			get => _columns;
			set => Setter
				.Set(ref _columns, value.ToArray())
				.Then(Initialize);
		}

		private IReadOnlyCollection<T> _data = new T[0];
		public IReadOnlyCollection<T> Data
		{
			get => _data;
			set => Setter
				.Set(ref _data, value)
				.Then(Initialize);
		}

		private DataGridStyle _style = DataGridStyle.AllBorders;
		public DataGridStyle Style
		{
			get => _style;
			set => Setter
				.Set(ref _style, value)
				.Then(Initialize);
		}

		private bool _showHeader = true;
		public bool ShowHeader
		{
			get => _showHeader;
			set => Setter
				.Set(ref _showHeader, value)
				.Then(Initialize);
		}

		public void Update()
		{
			Redraw();
		}

		public void Update(int row)
		{
			Update(new Rect(0, (row + 1) * 2, Size.Width, 1));
		}

		public void Update(int row, int column)
		{
			if (column >= Columns.Length) return;

			int xOffset = 0;
			for (int c = 0; c < column; c++) xOffset += Columns[c].Width + 1;

			Update(new Rect(xOffset, (row + 1) * 2, Columns[column].Width, 1));
		}

		public override Cell this[Position position]
		{
			get
			{
				if (position.Y < 0) return Character.Empty;
				if (position.X < 0) return Character.Empty;
				if (position.Y >= CalculateDesiredHeight()) return Character.Empty;
				if (position.X >= CalculateDesiredWidth()) return Character.Empty;

				int column = 0;
				int xOffset = position.X;
				bool isVerticalBorder = false;

				while (xOffset >= Columns[column].Width)
				{
					if (Style.HasVertivalBorders && xOffset == Columns[column].Width)
					{
						isVerticalBorder = true;
						break;
					}

					xOffset -= Columns[column].Width;
					xOffset -= Style.HasVertivalBorders ? 1 : 0;
					column += 1;
				}

				if (ShowHeader && position.Y == 0 && isVerticalBorder) return Style.HeaderVerticalBorder.Value;
				if (ShowHeader && position.Y == 1 && isVerticalBorder && Style.HeaderIntersectionBorder.HasValue) return Style.HeaderIntersectionBorder.Value;
				if (ShowHeader && position.Y == 1 && Style.HeaderHorizontalBorder.HasValue) return Style.HeaderHorizontalBorder.Value;
				if (ShowHeader && position.Y == 0) return Columns[column].GetHeader(xOffset);

				int yOffset = position.Y;

				if (ShowHeader) yOffset--;
				if (ShowHeader && Style.HeaderHorizontalBorder.HasValue) yOffset--;

				int row = Style.CellHorizontalBorder.HasValue
					? yOffset / 2
					: yOffset;
				bool isHorizontalBorder = Style.CellHorizontalBorder.HasValue
					? yOffset % 2 == 1
					: false;

				if (isHorizontalBorder && isVerticalBorder) return Style.CellIntersectionBorder.Value;
				if (isHorizontalBorder) return Style.CellHorizontalBorder.Value;
				if (isVerticalBorder) return Style.CellVerticalBorder.Value;

				return Columns[column].GetValue(Data.ElementAt(row), xOffset);
			}
		}

		protected override void Initialize()
		{
			Resize(new Size(CalculateDesiredWidth(), CalculateDesiredHeight()));
		}

		private int CalculateDesiredHeight()
		{
			int height = Data.Count;

			if (ShowHeader)
				height += 1;
			if (ShowHeader && Style.HeaderHorizontalBorder.HasValue)
				height += 1;
			if (Data.Count > 0 && Style.CellHorizontalBorder.HasValue)
				height += Data.Count - 1;

			return height;
		}

		private int CalculateDesiredWidth()
		{
			int width = Columns.Sum(c => c.Width);

			if (Columns.Length > 0 && Style.HasVertivalBorders)
				width += Columns.Length - 1;

			return width;
		}
	}
}
