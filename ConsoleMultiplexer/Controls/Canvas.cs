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
		private DrawingContext[,] _zBuffer = new DrawingContext[0, 0];

		public void Add(IControl control, Rect rect)
		{
			using (Freeze())
			{
				var newChild = new DrawingContext(this, control);
				newChild.SetOffset(rect.Offset);
				newChild.SetLimits(rect.Size, rect.Size);

				_children.Add(newChild);

				AddToZBuffer(newChild);
				Update(rect);
			}
		}

		public override Character this[Position position]
		{
			get
			{
				if (!Size.Contains(position)) return Character.Empty;
				if (_zBuffer[position.X, position.Y] == null) return Character.Empty;

				return _zBuffer[position.X, position.Y][position];
			}
		}

		protected override void Initialize()
		{
			if (MinSize == Size) return;

			using (Freeze())
			{
				Resize(MinSize);
				UpdateZBuffer();
			}
		}

		private void UpdateZBuffer()
		{
			_zBuffer = new DrawingContext[Size.Width, Size.Height];

			foreach (var child in _children)
				AddToZBuffer(child);
		}

		private void AddToZBuffer(DrawingContext drawingContext)
		{
			var rect = drawingContext.MinSize.AsRect().Move(drawingContext.Offset);

			foreach (var position in Rect.Intersect(rect, Size.Of(_zBuffer).AsRect()))
				_zBuffer[position.X, position.Y] = drawingContext;
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
