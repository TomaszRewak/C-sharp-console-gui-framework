using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class WrapPanel : Control, IDrawingContextListener
	{
		private readonly List<DrawingContext> _children = new List<DrawingContext>();
		public IEnumerable<IControl> Children
		{
			get => _children.Select(c => c.Child);
			set
			{
				foreach (var child in _children) child.Dispose();
				foreach (var child in value) _children.Add(new DrawingContext(this, child));

				Initialize();
			}
		}

		public override Character this[Position position]
		{
			get
			{
				var localPosition = position.UnWrap(Size.Width);

				foreach (var child in _children)
					if (child.Contains(localPosition))
						return child[localPosition];

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
						new Size(0, 1),
						new Size(Math.Max(0, MaxSize.Width * MaxSize.Height - left), 1));

					left += child.Size.Width;
				}

				Resize(new Size(left, 1));
			}
		}

		void IDrawingContextListener.OnRedraw(DrawingContext drawingContext)
		{
			Initialize();
		}

		void IDrawingContextListener.OnUpdate(DrawingContext drawingContext, Rect rect)
		{
			var begin = rect.LeftTopCorner.Wrap(Size.Width);
			var end = rect.RightBottomCorner.Wrap(Size.Width);

			Update(new Rect(0, begin.Y, Size.Width, end.Y - begin.Y + 1));
		}
	}
}
