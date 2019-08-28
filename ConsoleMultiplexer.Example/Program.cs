using ConsoleMultiplexer.Controls;
using System;
using System.Threading;

namespace ConsoleMultiplexer.Example
{
	class TestContext : IDrawingContext
	{
		public Size MinSize => new Size(30, 30);

		public Size MaxSize => new Size(30, 30);

		public void Update(IControl control)
		{
			for (int x = 0; x < control.Size.Width; x++)
			{
				for (int y = 0; y < control.Size.Width; y++)
				{
					Console.SetCursorPosition(x, y);
					Console.Write(control[Position.At(x, y)].Content);
				}
			}
		}

		public void Update(IControl control, in Position position)
		{
			Console.SetCursorPosition(position.X, position.Y);
			Console.Write(control[position].Content);
		}

		public event SizeLimitsChangedHandler SizeLimitsChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.SetWindowSize(1, 1);
			Console.SetBufferSize(200, 5);
			Console.SetWindowSize(200, 50);

			var testContext = new TestContext();

			var border = new Border();

			border.Context = testContext;
			testContext.Update(border);

			for (int i = 0; ; i++)
			{

				Thread.Sleep(500);
			}
		}
	}
}
