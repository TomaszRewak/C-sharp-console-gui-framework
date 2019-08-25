using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	internal class DisposableDrawingContext : IDrawingContext, IDisposable
	{
		private IDrawingContext _drawingContext;

		public DisposableDrawingContext(IDrawingContext drawingContext)
		{
			_drawingContext = drawingContext;
		}

		public void Dispose()
		{
			_drawingContext = null;
		}

		public void Clear()
		{
			_drawingContext?.Clear();
		}

		public void Flush()
		{
			_drawingContext?.Flush();
		}

		public void Set(in Position position, in Character character)
		{
			_drawingContext?.Set(position, character);
		}
	}
}
