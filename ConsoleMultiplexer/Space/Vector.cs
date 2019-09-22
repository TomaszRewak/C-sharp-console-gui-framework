using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Space
{
	public struct Vector
	{
		public int X { get; }
		public int Y { get; }

		public Vector(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static Vector operator -(in Vector vector) => new Vector(-vector.X, -vector.Y);
	}
}
