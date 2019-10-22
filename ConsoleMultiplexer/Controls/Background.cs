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

		private Character _fill;
		public Character Fill
		{
			get => _fill;
			set => Setter
				.Set(ref _fill, value)
				.Then(Redraw);
		}

		public bool _important;
		public bool Important
		{
			get => _important;
			set => Setter
				.Set(ref _important, value)
				.Then(Redraw);
		}

		public override Character this[Position position]
		{
			get
			{
				if (!ContentContext.Contains(position)) return Fill;

				var character = ContentContext[position];

				if (!character.Content.HasValue)
					character = character.WithContent(Fill.Content).WithForeground(Fill.Foreground);

				if (!character.Background.HasValue || Important)
					character = character.WithBackground(Fill.Background);

				return character;
			}
		}

		protected override void Initialize()
		{
			using (Freeze())
			{
				ContentContext.SetLimits(MinSize, MaxSize);

				Resize(ContentContext.Size);
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
