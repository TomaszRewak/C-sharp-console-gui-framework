using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Input
{
	public interface IInputListener
	{
		void OnInput(InputEvent inputEvent);
	}
}
