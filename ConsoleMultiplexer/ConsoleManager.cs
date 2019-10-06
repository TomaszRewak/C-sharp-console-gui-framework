using ConsoleMultiplexer.Buffering;
using ConsoleMultiplexer.Common;
using ConsoleMultiplexer.Controls;
using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Space;
using ConsoleMultiplexer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public class ConsoleManager : IDrawingContextListener
	{
		private readonly ConsoleBuffer _buffer = new ConsoleBuffer();
		private FreezeLock freezeLock;

		private DrawingContext _contentContext = DrawingContext.Dummy;
		private DrawingContext ContentContext
		{
			get => _contentContext;
			set => Setter
				.Set(ref _contentContext, value)
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

		//Character[,] _memory = new Character[100, 30];

		private void Initialize()
		{
			var consoleSize = new Size(Console.WindowWidth, Console.WindowHeight);

			_buffer.Initialize(consoleSize);
			Console.Clear();

			freezeLock.Freeze();
			ContentContext.SetLimits(consoleSize, consoleSize);
			freezeLock.Unfreeze();

			Redraw();
		}

		private void Redraw()
		{
			Update(ContentContext.Size.AsRect());
		}

		private void Update(Rect rect)
		{
			rect = ClipRect(rect);

			Console.CursorVisible = false;

			foreach (var position in rect)
			{
				var character = ContentContext[position];

				if (!_buffer.Update(position, character)) continue;

				var content = character.Content ?? ' ';
				var foreground = character.Foreground ?? Color.White;
				var background = character.Background ?? Color.Black;

				try
				{
					Console.SetCursorPosition(position.X, position.Y);
					Console.Write($"\x1b[38;2;{foreground.Red};{foreground.Green};{foreground.Blue}m\x1b[48;2;{background.Red};{background.Green};{background.Blue}m{content}");
				}
				catch (ArgumentOutOfRangeException)
				{ }
			}
		}

		public void AdjustSize()
		{
			Initialize();
		}

		private void BindContent()
		{
			ContentContext = new DrawingContext(this, Content);
		}

		private static Rect ClipRect(in Rect rect) => Rect.Intersect(rect, new Rect(0, 0, Console.WindowWidth, Console.WindowHeight));

		void IDrawingContextListener.OnRedraw(DrawingContext drawingContext)
		{
			if (freezeLock.IsFrozen) return;

			Redraw();
		}

		void IDrawingContextListener.OnUpdate(DrawingContext drawingContext, Rect rect)
		{
			if (freezeLock.IsFrozen) return;

			Update(rect);
		}
	}
}
