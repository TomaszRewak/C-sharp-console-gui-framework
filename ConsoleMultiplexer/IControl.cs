using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	internal interface IControl
	{
		IDrawingContext Context { get; set; }

		void Draw();
	}
}
