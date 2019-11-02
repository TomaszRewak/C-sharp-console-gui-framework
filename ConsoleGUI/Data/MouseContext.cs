using ConsoleGUI.Input;
using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Data
{
	public readonly struct MouseContext
	{
		public readonly IMouseListener MouseListener;
		public readonly Position RelativePosition;

		public MouseContext(IMouseListener mouseListener, in Position relativePosition)
		{
			MouseListener = mouseListener;
			RelativePosition = relativePosition;
		}
	}
}
