using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public delegate void SizeChanged(IDrawingContext drawingContext);

	public interface IDrawingContext
	{
		Size MinSize { get; }
		Size MaxSize { get; }

		void Set(in Position position, in Character character);
		void Flush();
		void Clear();

		event SizeChanged SizeChanged;
	}
}
