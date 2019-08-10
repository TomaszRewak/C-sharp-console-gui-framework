using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	internal class StringControl : IControl
	{
		public CursorPosition Start { get; private set; }
		public CursorPosition End { get; private set; }

		public void SetPosition(in CursorPosition cursorPosition, in ScreenSize screenSize)
		{

		}

		public char Get(in CursorPosition cursorPosition)
		{
			throw new NotImplementedException();
		}
	}
}
