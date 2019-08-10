using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	internal interface IControl
	{
		CursorPosition Start { get; }
		CursorPosition End { get; }

		void SetPosition(in CursorPosition cursorPosition, in ScreenSize screenSize);

		char Get(in CursorPosition cursorPosition);
	}
}
