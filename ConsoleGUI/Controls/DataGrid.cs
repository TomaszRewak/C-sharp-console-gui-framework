using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleGUI.Controls
{
	public class DataGrid<T> : Control
	{
		public class ColumnDefinition
		{
			public readonly string Header;
			public readonly int Width;
			public readonly Func<T, int, Character> Selector;

			public ColumnDefinition(string header, int width, Func<T, int, Character> selector)
			{
				Header = header;
				Width = width;
				Selector = selector;
			}

			public ColumnDefinition(string header, int width, Func<T, string> selector)
			{
				Header = header;
				Width = width;
				Selector = (v, i) =>
				{
					var text = selector(v);
					return i < text.Length ? new Character(text[i]) : Character.Empty;
				};
			}

			public ColumnDefinition(string header, int width, Func<T, string> selector, Func<T, Color?> foreground = null, Func<T, Color?> background = null)
			{
				Header = header;
				Width = width;
				Selector = (v, i) =>
				{
					var text = selector(v);
					return new Character(i < text.Length ? (char?)text[i] : null, foreground?.Invoke(v), background?.Invoke(v));
				};
			}
		}

		private ColumnDefinition[] columns = new ColumnDefinition[0];
		public ColumnDefinition[] Columns
		{
			get => columns;
			set => Setter
				.Set(ref columns, value.ToArray())
				.Then(Initialize);
		}

		private IReadOnlyCollection<T> data = new T[0];
		public IReadOnlyCollection<T> Data
		{
			get => data;
			set => Setter
				.Set(ref data, value)
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
				int column = 0;
				int xOffset = 0;
				if (position.Y > Data.Count * 2) return Character.Empty;
				while (column < Columns.Length && position.X > xOffset + Columns[column].Width) xOffset += Columns[column++].Width + 1;

				int x = position.X - xOffset;
				if (column >= Columns.Length) return Character.Empty;
				if (column == Columns.Length - 1 && x == Columns[column].Width) return Character.Empty;
				if (position.Y == 1 && x == Columns[column].Width) return new Character('╪');
				if (position.Y == 1) return new Character('═');
				if (position.Y % 2 == 1 && x == Columns[column].Width) return new Character('┼');
				if (position.Y % 2 == 1) return new Character('─');
				if (x == Columns[column].Width) return new Character('│');
				if (position.Y == 0) return x < Columns[column].Header.Length ? new Character(Columns[column].Header[x]) : Character.Empty;
				return Columns[column].Selector(Data.ElementAt(position.Y / 2 - 1), x);
			}
		}

		protected override void Initialize()
		{
			using (Freeze())
			{
				int width = Columns.Sum(c => c.Width) + Math.Max(0, Columns.Length - 1);
				int height = Data.Count * 2 + 1;

				Resize(new Size(width, height));
			}
		}
	}
}
