using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public class InputEvent
	{
		public ConsoleKeyInfo Key { get; }

		public InputEvent(ConsoleKeyInfo key)
		{
			Key = key;
		}

		public void Handled()
		{

		}
	}
}
