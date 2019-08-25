using System;

namespace ConsoleMultiplexer
{
	[Flags]
	public enum BorderPlacement
	{
		None   = 0b0000,
		Left   = 0b0001,
		Top    = 0b0010,
		Right  = 0b0100,
		Bottom = 0b1000,
		All    = 0b1111
	}
}
