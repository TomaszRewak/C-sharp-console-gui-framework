using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Input
{
	public class InputEvent
	{
		public ConsoleKeyInfo Key { get; }
		public bool Handled { get; set; }

		public InputEvent(ConsoleKeyInfo key)
		{
			Key = key;
		}
	}
}
