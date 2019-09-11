using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public interface IInputListener
	{
		void OnInput(InputEvent inputEvent);
	}
}
