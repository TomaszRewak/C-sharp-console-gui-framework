using System;

namespace ConsoleMultiplexer
{
	[Flags]
	public enum BorderPlacement
	{
		None = 0b0000,
		Left = 0b0001,
		Top = 0b0010,
		Right = 0b0100,
		Bottom = 0b1000,
		All = 0b1111
	}

	public static class BorderPalcementExtension
	{
		public static bool HasBorder(this BorderPlacement self, BorderPlacement border) => (self & border) == border;

		public static int Offset(this BorderPlacement self, BorderPlacement border) => HasBorder(self, border) ? 1 : 0;

		public static Offset AsOffset(this BorderPlacement self)
		{
			return new Offset(
				self.Offset(BorderPlacement.Left),
				self.Offset(BorderPlacement.Top),
				self.Offset(BorderPlacement.Right),
				self.Offset(BorderPlacement.Bottom));
		}

		public static Vector AsVector(this BorderPlacement self)
		{
			return new Vector(
				self.Offset(BorderPlacement.Left),
				self.Offset(BorderPlacement.Top));
		}
	}
}
