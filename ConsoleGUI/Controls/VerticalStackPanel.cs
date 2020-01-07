using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGUI.Controls
{
	public class VerticalStackPanel : Control, IDrawingContextListener
	{
		private readonly List<DrawingContext> _children = new List<DrawingContext>();
		public IEnumerable<IControl> Children
		{
			get => _children.Select(c => c.Child);
			set
			{
				foreach (var child in _children) child.Dispose();

				_children.Clear();
				
				foreach (var child in value) _children.Add(new DrawingContext(this, child));

				Initialize();
			}
		}

		public void Add(IControl control)
		{
			using (Freeze())
			{
				_children.Add(new DrawingContext(this, control));

				Initialize();
			}
		}

		public void Remove(IControl control)
		{

			using (Freeze())
			{
				var child = _children.FirstOrDefault(c => c.Child == control);

				if (child == null) return;

				child.Dispose();
				_children.Remove(child);

				Initialize();
			}
		}

		public override Cell this[Position position]
		{
			get
			{
				foreach (var child in _children)
					if (child.Contains(position))
						return child[position];

				return Character.Empty;
			}
		}

		protected override void Initialize()
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

				Resize(new Size(MaxSize.Width, top));
			}
		}

		void IDrawingContextListener.OnRedraw(DrawingContext drawingContext)
		{
			Initialize();
		}

		void IDrawingContextListener.OnUpdate(DrawingContext drawingContext, Rect rect)
		{
			Update(rect);
		}
	}
}
