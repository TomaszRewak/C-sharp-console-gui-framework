using ConsoleGUI.Input;
using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Data
{
	public readonly struct MouseListener
	{
		public readonly IMouseListener Listener;
		public readonly Position Position;

		public MouseListener(IMouseListener listener, in Position position)
		{
			Listener = listener;
			Position = position;
		}
	}
}
