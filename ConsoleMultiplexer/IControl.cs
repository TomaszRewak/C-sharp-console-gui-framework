using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	internal interface IControl
	{
		void Draw(IDrawingContext context);
	}
}
