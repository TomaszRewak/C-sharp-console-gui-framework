using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public delegate void SizeChangedHandler(IControl control);
	public delegate void CharacterChangedHandler(IControl control, Position position);

	public interface IControl
	{
		IDrawingContext Context { set; }
		Size Size { get; }

		Character this[Position position] { get; }
	}
}
