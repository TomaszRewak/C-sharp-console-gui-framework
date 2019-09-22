using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Space
{
	public struct Rect
	{
		public int Left { get; }
		public int Top { get; }
		public int Width { get; }
		public int Height { get; }

		public bool IsEmpty => Width == 0 && Height == 0;
		public int Right => Left + Width - 1;
		public int Bottom => Top + Height - 1;
		public Position LeftTopCorner => new Position(Left, Top);
		public Position RightBottomCorner => new Position(Right, Bottom);
		public Size Size => new Size(Width, Height);
		public Vector Offset => new Vector(Left, Top);

		public Rect(int left, int top, int width, int height)
		{
			Left = left;
			Top = top;
			Width = width;
			Height = height;
		}

		public static Rect Empty => new Rect(0, 0, 0, 0);

		public static Rect Containing(in Position position) => new Rect(position.X, position.Y, 1, 1);
		public static Rect OfSize(in Size size) => new Rect(0, 0, size.Width, size.Height);

		public static Rect Surround(in Rect lhs, in Rect rhs)
		{
			if (rhs.IsEmpty) return lhs;
			return lhs.ExtendBy(rhs.LeftTopCorner).ExtendBy(rhs.RightBottomCorner);
		}

		public static Rect Intersect(in Rect lhs, in Rect rhs)
		{
			if (lhs.IsEmpty || rhs.IsEmpty) return Empty;

			var left = Math.Max(lhs.Left, rhs.Left);
			var top = Math.Max(lhs.Top, rhs.Top);
			var width = Math.Min(lhs.Right, rhs.Right) - left + 1;
			var height = Math.Min(lhs.Bottom, rhs.Bottom) - top + 1;

			return new Rect(left, top, width, height);
		}

		public Rect ExtendBy(in Position position)
		{
			if (IsEmpty) return Containing(position);

			var left = Math.Min(Left, position.X);
			var top = Math.Min(Top, position.Y);
			var width = Math.Max(Right, position.X) - left + 1;
			var height = Math.Max(Bottom, position.Y) - top + 1;

			return new Rect(left, top, width, height);
		}

		public Rect Remove(in Offset offset)
		{
			return new Rect(
				Left + offset.Left,
				Top + offset.Top,
				Math.Max(0, Width - offset.Left - offset.Right),
				Math.Max(0, Height - offset.Top - offset.Bottom));
		}

		public Rect Add(in Offset offset)
		{
			return new Rect(
				Left + offset.Left,
				Top + offset.Top,
				Math.Max(0, Width + offset.Left + offset.Right),
				Math.Max(0, Height + offset.Top + offset.Bottom));
		}

		public Rect Move(int x, int y)
		{
			return new Rect(
				Left + x,
				Top + y,
				Width,
				Height);
		}

		public Rect Move(in Vector vector) => Move(vector.X, vector.Y);

		public IEnumerator<Position> GetEnumerator()
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					yield return new Position(x + Left, y + Top);
		}
	}
}
