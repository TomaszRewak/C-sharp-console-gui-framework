using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	internal interface IControl
	{
		CursorPosition Start { get; set; }
		CursorPosition End { get; }

		Character Get(in CursorPosition cursorPosition);
	}
}
