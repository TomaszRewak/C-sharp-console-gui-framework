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
	public class Grid : Control, IDrawingContextListener
	{
		public readonly struct ColumnDefinition
		{
			public readonly int Width;

			public ColumnDefinition(int width)
			{
				Width = width;
			}
		}

		public readonly struct RowDefinition
		{
			public readonly int Height;

			public RowDefinition(int height)
			{
				Height = height;
			}
		}

		private DrawingContext[,] children = new DrawingContext[0, 0];
		private DrawingContext[,] Children
		{
			get => children;
			set => Setter
				.Set(ref children, value);
		}

		private ColumnDefinition[] _columns = new ColumnDefinition[0];
		public ColumnDefinition[] Columns
		{
			get => _columns;
			set => Setter
				.Set(ref _columns, value.ToArray())
				.Then(ResizeBuffer)
				.Then(Initialize);
		}

		private RowDefinition[] _rows = new RowDefinition[0];
		public RowDefinition[] Rows
		{
			get => _rows;
			set => Setter
				.Set(ref _rows, value.ToArray())
				.Then(ResizeBuffer)
				.Then(Initialize);
		}

		public void AddChild(int x, int y, IControl control)
		{
			using (Freeze())
			{
				ref var child = ref Children[x, y];
				var rect = GetRect(x, y);

				child?.Dispose();
				child = new DrawingContext(this, control);
				child.SetOffset(rect.Offset);
				child.SetLimits(rect.Size, rect.Size);

				Update(rect);
			}
		}

		public bool HasChild(int x, int y)
		{
			return Children[x, y] != null;
		}

		public IControl GetChild(int x, int y)
		{
			return Children[x, y].Child;
		}

		public void RemoveChild(int x, int y)
		{
			Children[x, y]?.Dispose();
			Children[x, y] = null;

			Update(GetRect(x, y));
		}

		public override Character this[Position position]
		{
			get
			{
				int x = 0;
				int y = 0;

				for (int xOffset = 0; x < Columns.Length && position.X >= xOffset + Columns[x].Width; xOffset += Columns[x++].Width) ;
				for (int yOffset = 0; y < Rows.Length && position.Y >= yOffset + Rows[y].Height; yOffset += Rows[y++].Height) ;

				if (x >= Columns.Length || y >= Rows.Length || Children[x, y] == null) return Character.Empty;

				return Children[x, y][position];
			}
		}

		protected override void Initialize()
		{
			using (Freeze())
			{
				for (int x = 0, xOffset = 0; x < Columns.Length; xOffset += Columns[x++].Width)
				{
					for (int y = 0, yOffset = 0; y < Rows.Length; yOffset += Rows[y++].Height)
					{
						var child = Children[x, y];
						var size = new Size(Columns[x].Width, Rows[y].Height);

						child?.SetOffset(new Vector(xOffset, yOffset));
						child?.SetLimits(size, size);
					}
				}

				int width = 0,
					height = 0;

				for (int x = 0; x < Columns.Length; x++) width += Columns[x].Width;
				for (int y = 0; y < Rows.Length; y++) height += Rows[y].Height;

				Resize(new Size(width, height));
			}
		}

		private void ResizeBuffer()
		{
			var newBuffer = new DrawingContext[Columns.Length, Rows.Length];

			for (int x = 0; x < Children.GetLength(0); x++)
			{
				for (int y = 0; y < Children.GetLength(1); y++)
				{
					if (x < Columns.Length && y < Columns.Length)
						newBuffer[x, y] = Children[x, y];
					else
						Children[x, y]?.Dispose();
				}
			}

			Children = newBuffer;
		}

		private Rect GetRect(int x, int y)
		{
			var size = new Size(Columns[x].Width, Rows[y].Height);

			int xOffset = 0;
			int yOffset = 0;

			while (--x >= 0) xOffset += Columns[x].Width;
			while (--y >= 0) yOffset -= Rows[y].Height;

			return new Rect(new Vector(xOffset, yOffset), size);
		}

		void IDrawingContextListener.OnRedraw(DrawingContext drawingContext)
		{
			Update(new Rect(drawingContext.Offset, drawingContext.MaxSize));
		}

		void IDrawingContextListener.OnUpdate(DrawingContext drawingContext, Rect rect)
		{
			Update(rect);
		}
	}
}
