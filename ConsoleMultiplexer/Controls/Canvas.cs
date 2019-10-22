using ConsoleMultiplexer.Common;
using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Space;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class Canvas : Control, IDrawingContextListener
	{
		private readonly List<DrawingContext> _children = new List<DrawingContext>();

		public void Add(IControl control, Rect rect)
		{
			using (Freeze())
			{
				var newChild = new DrawingContext(this, control);
				newChild.SetOffset(rect.Offset);
				newChild.SetLimits(rect.Size, rect.Size);

				_children.Insert(0, newChild);

				Update(rect);
			}
		}

		public override Character this[Position position]
		{
			get
			{
				if (!Size.Contains(position)) return Character.Empty;

				foreach (var child in _children)
					if (child.Contains(position))
						return child[position];

				return Character.Empty;
			}
		}

		protected override void Initialize()
		{
			Resize(MinSize);
		}

		void IDrawingContextListener.OnRedraw(DrawingContext drawingContext)
		{
			Update(drawingContext.MinSize.AsRect().Move(drawingContext.Offset));
		}

		void IDrawingContextListener.OnUpdate(DrawingContext drawingContext, Rect rect)
		{
			Update(rect);
		}
	}
}
