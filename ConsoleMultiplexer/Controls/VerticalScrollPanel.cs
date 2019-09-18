using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class VerticalScrollPanel : Control, IDrawingContextListener
	{
		private DrawingContext _contentContext = DrawingContext.Dummy;
		private DrawingContext ContentContext
		{
			get => _contentContext;
			set => Setter
				.SetDisposable(ref _contentContext, value)
				.Then(Initialize);
		}

		private IControl _content;
		public IControl Content
		{
			get => _content;
			set => Setter
				.Set(ref _content, value)
				.Then(BindContent);
		}

		private int _top;
		public int Top
		{
			get => _top;
			set => Setter
				.Set(ref _top, value)
				.Then(Initialize);
		}

		public override Character this[Position position]
		{
			get
			{
				if (position.X != Size.Width - 1)
					return ContentContext[position];

				if (Content == null) return new Character('#');
				if (Content.Size.Height <= Size.Height) return new Character('#');
				if (position.Y * Content.Size.Height < Top * Size.Height) return Character.Empty;
				if (position.Y * Content.Size.Height > (Top + Size.Height) * Size.Height) return Character.Empty;

				return new Character('#');
			}
		}

		protected override void Initialize()
		{
			using(Freeze())
			{
				ContentContext.SetLimits(MaxSize.Shrink(1, 0), MaxSize.Shrink(1, 0).WithInfitineHeight);
				ContentContext.SetOffset(new Vector(0, -Top));

				Resize(Size.Clip(MinSize, ContentContext.Size.Expand(1, 0), MaxSize));
			}
		}

		private void BindContent()
		{
			ContentContext = new DrawingContext(this, Content);
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
