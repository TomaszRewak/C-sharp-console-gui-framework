using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public delegate void SizeChanged(IDrawingContext drawingContext);

	public interface IDrawingContext
	{
		Size Size { get; set; }

		void Set(in Position position, in Character character);
		void Clear();
	}
}
