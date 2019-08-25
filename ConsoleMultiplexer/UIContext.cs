using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public class UIContext
	{
		private readonly IDrawingContext _drawingContext;

		public readonly Size MinSize;
		public readonly Size MaxSize;

		public UIContext(IDrawingContext drawingContext, Size minSize, Size maxSize)
		{
			_drawingContext = drawingContext;

			MinSize = minSize;
			MaxSize = maxSize;
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
			if (MaxSize.Contains(position))
				_drawingContext?.Set(position, character);
		}

		internal static UIContext Empty => new UIContext(null, Size.Empty, Size.Empty);
	}
}
