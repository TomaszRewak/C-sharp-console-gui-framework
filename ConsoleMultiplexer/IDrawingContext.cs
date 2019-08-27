using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public delegate void SizeLimitsChangedHandler(IDrawingContext drawingContext);

	public interface IDrawingContext
	{
		Size MinSize { get; }
		Size MaxSize { get; }

		void Update();
		void Update(in Position position);

		event SizeLimitsChangedHandler SizeLimitsChanged;
	}
}
