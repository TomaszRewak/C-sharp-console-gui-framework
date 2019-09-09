using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	internal class DrawingContext : IDrawingContext, IDisposable
	{
		private Action _onRedrawRequested;
		private Action<Rect> _onUpdateRequested;

		public IControl Control { get; }

		public DrawingContext(IControl control, Action onRedrawRequested, Action<Rect> onUpdateRequested)
		{
			if (control == null) return;

			_onRedrawRequested = onRedrawRequested;
			_onUpdateRequested = onUpdateRequested;

			Control = control;
			Control.Context = this;
		}

		public void Dispose()
		{
			_onRedrawRequested = null;
			_onUpdateRequested = null;
		}

		public Size MinSize { get; private set; }
		public Size MaxSize { get; private set; }

		public Vector Offset { get; private set; }

		public Character this[Position position]
		{
			get
			{
				return Control[position.Move(-Offset)];
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
			Offset = offset;
		}

		public bool Contains(in Position position)
		{
			return Control.Size.Contains(position.Move(-Offset));
		}

		public void Redraw(IControl control)
		{
			if (control != Control) return;

			_onRedrawRequested?.Invoke();
		}

		public void Update(IControl control, in Rect rect)
		{
			if (control != Control) return;

			_onUpdateRequested?.Invoke(rect.Move(Offset));
		}

		public event SizeLimitsChangedHandler SizeLimitsChanged;
	}
}
