using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class VerticalStackPanel : Control, IDrawingContextListener
	{
		private readonly List<DrawingContext> _children = new List<DrawingContext>();

		public void Add(IControl control)
		{
			using (Freeze())
			{
				_children.Add(new DrawingContext(this, control));

				Resize();
			}
		}

		public void Remove(IControl control)
		{

			using (Freeze())
			{
				_children.RemoveAll(c => c.Child == control);

				Resize();
			}
		}

		public override Character this[Position position]
		{
			get
			{
				foreach (var child in _children)
					if (child.Contains(position))
						return child[position];

				return Character.Empty;
			}
		}

		protected override void Resize()
		{
			using (Freeze())
			{
				int top = 0;
				foreach (var child in _children)
				{
					child.SetOffset(new Vector(0, top));
					child.SetLimits(
						new Size(MaxSize.Width, 0),
						new Size(MaxSize.Width, Math.Max(0, MaxSize.Height - top)));

					top += child.Child.Size.Height;
				}

				Redraw(new Size(MaxSize.Width, top));
			}
		}

		void IDrawingContextListener.OnRedraw(DrawingContext drawingContext)
		{
			Resize();
		}

		void IDrawingContextListener.OnUpdate(DrawingContext drawingContext, Rect rect)
		{
			Update(rect);
		}
	}
}
