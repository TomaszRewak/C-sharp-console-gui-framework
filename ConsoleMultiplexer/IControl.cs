using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public interface IControl
	{
		void Draw(UIContext drawingContext);
	}
}
