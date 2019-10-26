using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Space;
using System;
using System.Runtime.CompilerServices;

namespace ConsoleMultiplexer.Common
{
	internal interface IDrawingContextListener
	{
		void OnRedraw(DrawingContext drawingContext);
		void OnUpdate(DrawingContext drawingContext, Rect rect);
	}

	internal class DrawingContext : IDrawingContext, IDisposable
	{
		public IDrawingContextListener Parent { get; private set; }
		public IControl Child { get; private set; }

		public DrawingContext(IDrawingContextListener parent, IControl control)
		{
			if (control == null) return;

			Parent = parent;

			Child = control;
			Child.Context = this;
		}

		public static DrawingContext Dummy => new DrawingContext(null, null);

		public void Dispose()
		{
			Parent = null;
			Child = null;
		}

		public Size MinSize { get; private set; }
		public Size MaxSize { get; private set; }
		public Vector Offset { get; private set; }

		public Size Size => Child?.Size ?? Size.Empty;

		public Character this[Position position]
		{
			get
			{
				return Child[position.Move(-Offset)];
			}
		}

		public void SetLimits(in Size minSize, in Size maxSize)
		{
			if (MinSize == minSize && MaxSize == maxSize) return;

			MinSize = minSize;
			MaxSize = maxSize;

			SizeLimitsChanged?.Invoke(this);
		}

		public void SetOffset(in Vector offset)
		{
			if (offset == Offset) return;

			Update(Child, Rect.OfSize(Size));
			Offset = offset;
			Update(Child, Rect.OfSize(Size));
		}

		public bool Contains(in Position position)
		{
			if (Child == null) return false;

			return Child.Size.Contains(position.Move(-Offset));
		}

		public void Redraw(IControl control)
		{
			Parent?.OnRedraw(this);
		}

		public void Update(IControl control, in Rect rect)
		{
			Parent?.OnUpdate(this, rect.Move(Offset));
		}

		public event SizeLimitsChangedHandler SizeLimitsChanged;
	}
}
