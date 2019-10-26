using System;
using System.Collections.Generic;
using System.Text;
using ConsoleGUI.Space;

namespace ConsoleGUI.UserDefined
{
	internal class DrawingContextWrapper : IDrawingContext, IDisposable
	{
		public IControl Parent { get; }
		public IControl Child { get; }
		public IDrawingContext Context { get; }
		
		public DrawingContextWrapper(IControl parent, IControl child, IDrawingContext context)
		{
			Parent = parent;
			Child = child;
			Context = context;

			if (Context != null)
				Context.SizeLimitsChanged += OnSizeLimitsChanged;
		}

		public void Dispose()
		{
			if (Context != null)
				Context.SizeLimitsChanged -= OnSizeLimitsChanged;
		}

		private void OnSizeLimitsChanged(IDrawingContext drawingContext)
		{
			SizeLimitsChanged?.Invoke(this);
		}

		public Size MinSize => Context?.MinSize ?? Size.Empty;
		public Size MaxSize => Context?.MaxSize ?? Size.Empty;

		public void Redraw(IControl control)
		{
			if (control != Child) return;

			Context?.Redraw(Parent);
		}

		public void Update(IControl control, in Rect rect)
		{
			if (control != Child) return;

			Context?.Update(Parent, rect);
		}

		public event SizeLimitsChangedHandler SizeLimitsChanged;
	}
}
