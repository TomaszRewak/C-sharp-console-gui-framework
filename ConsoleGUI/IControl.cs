using ConsoleGUI.Data;
using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI
{
	public delegate void SizeChangedHandler(IControl control);
	public delegate void CharacterChangedHandler(IControl control, Position position);

	public interface IControl
	{
		Cell this[Position position] { get; }

		Size Size { get; }

		IDrawingContext Context { get; set; }
	}
}
