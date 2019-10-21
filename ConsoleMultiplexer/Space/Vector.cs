using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Space
{
	public readonly struct Vector
	{
		public int X { get; }
		public int Y { get; }

		public Vector(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static Vector operator -(in Vector vector) => new Vector(-vector.X, -vector.Y);

		public static bool operator ==(in Vector lhs, in Vector rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;
		public static bool operator !=(in Vector lhs, in Vector rhs) => !(lhs == rhs);

		public override bool Equals(object obj) => obj is Vector vector && this == vector;

		public override int GetHashCode()
		{
			var hashCode = 1861411795;
			hashCode = hashCode * -1521134295 + X.GetHashCode();
			hashCode = hashCode * -1521134295 + Y.GetHashCode();
			return hashCode;
		}
	}
}
