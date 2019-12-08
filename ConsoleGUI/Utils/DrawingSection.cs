using System;
using System.Collections.Generic;
using System.Text;
using ConsoleGUI.Data;
using ConsoleGUI.Space;

namespace ConsoleGUI.Utils
{
	internal class DrawingSection : IControl
	{
		public IDrawingContext Context { get; set; }

		private IControl _content;
		public IControl Content
		{
			get => _content;
			set => Setter
				.Set(ref _content, value)
				.Then(Redraw);
		}

		private Rect _rect;
		public Rect Rect
		{
			get => _rect;
			set => Setter
				.Set(ref _rect, value)
				.Then(Redraw);
		}

		public Cell this[Position position]
		{
			get
			{
				if (Content == null) return Character.Empty;

				position = position.Move(Rect.TopLeftCorner.AsVector());

				if (!Rect.Contains(position)) return Character.Empty;
				if (!Content.Size.Contains(position)) return Character.Empty;

				return Content[position];
			}
		}

		public void Update(in Rect rect)
		{
			var intersection = Rect.Intersect(Rect, rect);

			if (intersection.IsEmpty) return;

			Context?.Update(this, intersection.Move(-Rect.TopLeftCorner.AsVector()));
		}

		public Size Size => Rect.Size;

		private void Redraw() => Context?.Redraw(this);
	}
}
