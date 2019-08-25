using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public interface IControl
	{
		IDrawingContext Context { get; set; }

		void Draw();
	}
}
