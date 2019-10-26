using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Data
{
	public readonly struct BorderStyle
	{
		public readonly Character Top;
		public readonly Character TopRight;
		public readonly Character Right;
		public readonly Character BottomRight;
		public readonly Character Bottom;
		public readonly Character BottomLeft;
		public readonly Character Left;
		public readonly Character TopLeft;

		public BorderStyle(
			in Character top,
			in Character topRight,
			in Character right,
			in Character bottomRight,
			in Character bottom,
			in Character bottomLeft,
			in Character left,
			in Character topLeft)
		{
			Top = top;
			TopRight = topRight;
			Right = right;
			BottomRight = bottomRight;
			Bottom = bottom;
			BottomLeft = bottomLeft;
			Left = left;
			TopLeft = topLeft;
		}

		public static BorderStyle Double => new BorderStyle(
			new Character('═'),
			new Character('╗'),
			new Character('║'),
			new Character('╝'),
			new Character('═'),
			new Character('╚'),
			new Character('║'),
			new Character('╔'));

		public static BorderStyle Single => new BorderStyle(
			new Character('─'),
			new Character('┐'),
			new Character('│'),
			new Character('┘'),
			new Character('─'),
			new Character('└'),
			new Character('│'),
			new Character('┌'));

		public BorderStyle WithColor(in Color foreground) => new BorderStyle(
			Top.WithForeground(foreground),
			TopRight.WithForeground(foreground),
			Right.WithForeground(foreground),
			BottomRight.WithForeground(foreground),
			Bottom.WithForeground(foreground),
			BottomLeft.WithForeground(foreground),
			Left.WithForeground(foreground),
			TopLeft.WithForeground(foreground));

		public BorderStyle WithTopLeft(in Character topLeft) => new BorderStyle(
			Top,
			TopRight,
			Right,
			BottomRight,
			Bottom,
			BottomLeft,
			Left,
			topLeft);

		public BorderStyle WithTopRight(in Character topRight) => new BorderStyle(
			Top,
			topRight,
			Right,
			BottomRight,
			Bottom,
			BottomLeft,
			Left,
			TopLeft);
	}
}
