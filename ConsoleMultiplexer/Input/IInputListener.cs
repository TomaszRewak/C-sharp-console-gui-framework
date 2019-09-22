using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Input
{
	public interface IInputListener
	{
		void OnInput(InputEvent inputEvent);
	}
}
