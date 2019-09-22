using ConsoleMultiplexer.Common;
using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Helpers;
using ConsoleMultiplexer.Space;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public class ConsoleManager : IDrawingContextListener, IDisposable
	{
		private DrawingContext _contentContext = DrawingContext.Dummy;
		private DrawingContext ContentContext
		{
			get => _contentContext;
			set => Setter
				.SetDisposable(ref _contentContext, value)
				.Then(Redraw);
		}

		private IControl _content;
		public IControl Content
		{
			get => _content;
			set => Setter
				.Set(ref _content, value)
				.Then(BindContent);
		}

		public Size Size { get; set; }

		public Size MinSize => new Size(100, 30);
		public Size MaxSize => new Size(100, 30);

		//Character[,] _memory = new Character[100, 30];

		private IControl _control;
		public IControl Control
		{
			get => _control;
			set
			{
				_control = value;
				_control.Context = this;
				Redraw(_control);
			}
		}

		public void Redraw()
		{
			if (control != _control) return;

			Update(control, control.Size.AsRect());
		}

		public void Update(Rect rect)
		{

		}

		public void Update(IControl control, in Rect rect)
		{
			if (control != _control) return;

			foreach (var position in rect)
			{
				var c = control[Position.At(position.X, position.Y)];

				//if (c.Content != _memory[position.X, position.Y].Content)
				{
					//_memory[position.X, position.Y] = c;

					var content = c.Content ?? ' ';
					var foreground = c.Foreground ?? Color.White;
					var background = c.Background ?? Color.Black;

					Console.SetCursorPosition(position.X, position.Y);
					Console.Write($"\x1b[38;2;{foreground.Red};{foreground.Green};{foreground.Blue}m\x1b[48;2;{background.Red};{background.Green};{background.Blue}m{content}");
				}
			}

			Console.BackgroundColor = ConsoleColor.Black;
		}

		private void BindContent()
		{
			ContentContext = new DrawingContext(this, Control);
		}

		public void OnRedraw(DrawingContext drawingContext)
		{
			Redraw();
		}

		public void OnUpdate(DrawingContext drawingContext, Rect rect)
		{
			Update(rect);
		}

		public event SizeLimitsChangedHandler SizeLimitsChanged;
	}
}
