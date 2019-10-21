using ConsoleMultiplexer.Common;
using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Space;
using ConsoleMultiplexer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class Background : Control, IDrawingContextListener
	{
		private DrawingContext _contentContext = DrawingContext.Dummy;
		private DrawingContext ContentContext
		{
			get => _contentContext;
			set => Setter
				.SetDisposable(ref _contentContext, value);
		}

		private IControl _content;
		public IControl Content
		{
			get => _content;
			set => Setter
				.Set(ref _content, value)
				.Then(BindContent);
		}

		private Character _fill;
		public Character Fill
		{
			get => _fill;
			set => Setter
				.Set(ref _fill, value)
				.Then(Redraw);
		}

		public override Character this[Position position]
		{
			get
			{
				if (!ContentContext.Contains(position)) return Fill;

				var character = ContentContext[position];

				if (!character.Content.HasValue) return Fill;

				if (character.Background == null)
					character = character.WithBackground(Fill.Background);

				return character;
			}
		}

		protected override void Initialize()
		{
			using (Freeze())
			{
				ContentContext.SetLimits(MinSize, MaxSize);

				Resize(Size.Clip(MinSize, ContentContext.Size, MaxSize));
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
