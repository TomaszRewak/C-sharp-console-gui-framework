using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Input
{
	public interface IMouseListener
	{
		void OnMouseEnter();
		void OnMouseLeave();
		void OnMouseUp(Position position);
		void OnMouseDown(Position position);
		void OnMouseMove(Position position);
	}
}
