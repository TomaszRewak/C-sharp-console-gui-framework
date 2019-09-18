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

		private Character _scrollBarForeground = new Character('▓', foreground: Color.LightBlue);
		public Character ScrollBarForeground
		{
			get => _scrollBarForeground;
			set => Setter
				.Set(ref _scrollBarForeground, value)
				.Then(RedrawScrollBar);
		}

		private Character _scrollBarBackground = new Character('▒', foreground: Color.Gray);
		public Character ScrollBarBackground
		{
			get => _scrollBarBackground;
			set => Setter
				.Set(ref _scrollBarBackground, value)
				.Then(RedrawScrollBar);
		}

		public override Character this[Position position]
		{
			get
			{
				if (position.X != Size.Width - 1)
					return ContentContext[position];

				if (Content == null) return ScrollBarForeground;
				if (Content.Size.Height <= Size.Height) return ScrollBarForeground;
				if (position.Y * Content.Size.Height < Top * Size.Height) return ScrollBarBackground;
				if (position.Y * Content.Size.Height > (Top + Size.Height) * Size.Height) return ScrollBarBackground;

				return ScrollBarForeground;
			}
		}

		protected override void Initialize()
		{
			using(Freeze())
			{
				ContentContext.SetLimits(MaxSize.Shrink(1, 0), MaxSize.Shrink(1, 0).WithInfitineHeight());
				ContentContext.SetOffset(new Vector(0, -Top));

				Resize(Size.Clip(MinSize, ContentContext.Size.Expand(1, 0), MaxSize));
			}
		}

		private void BindContent()
		{
			ContentContext = new DrawingContext(this, Content);
		}

		private void RedrawScrollBar()
		{
			Update(Size.WithWidth(1).AsRect().Move(Size.Width - 1, 0));
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
