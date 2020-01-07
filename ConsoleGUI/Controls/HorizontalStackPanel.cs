using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGUI.Controls
{
	public class HorizontalStackPanel : Control, IDrawingContextListener
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
				int left = 0;
				foreach (var child in _children)
				{
					child.SetOffset(new Vector(left, 0));
					child.SetLimits(
						new Size(0, MaxSize.Height),
						new Size(Math.Max(0, MaxSize.Width - left), MaxSize.Height));

					left += child.Child.Size.Width;
				}

				Resize(new Size(left, MaxSize.Height));
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
