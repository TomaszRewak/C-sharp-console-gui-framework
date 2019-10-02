using ConsoleMultiplexer.Common;
using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Space;
using ConsoleMultiplexer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	internal class ConsoleWrapper : Control, IDrawingContextListener
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

		public override Character this[Position position] => ContentContext[position];

		public void AdjustSize()
		{
			var consoleSize = new Size(Console.WindowWidth, Console.WindowHeight);

			ContentContext.SetLimits(consoleSize, consoleSize);
		}

		protected override void Initialize()
		{
			AdjustSize();
		}

		private void BindContent()
		{
			ContentContext = new DrawingContext(this, Content);
		}

		void IDrawingContextListener.OnRedraw(DrawingContext drawingContext)
		{
			Redraw();
		}

		void IDrawingContextListener.OnUpdate(DrawingContext drawingContext, Rect rect)
		{
			Update(rect);
		}
	}
}
