using ConsoleGUI.Input;
using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Data
{
	public readonly struct MouseListener
	{
		public readonly IMouseListener Control;
		public readonly Position RelativePosition;

		public MouseListener(IMouseListener control, in Position relativePosition)
		{
			Control = control;
			RelativePosition = relativePosition;
		}
	}
}
