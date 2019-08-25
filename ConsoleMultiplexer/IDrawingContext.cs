using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public delegate void SizeChanged(IDrawingContext drawingContext);

	public interface IDrawingContext
	{
		void Set(in Position position, in Character character);
		void Flush();
		void Clear(Size size);
	}
}
