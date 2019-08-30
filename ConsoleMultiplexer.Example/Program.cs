using ConsoleMultiplexer.Controls;
using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleMultiplexer.Example
{
	class TestContext : IDrawingContext
	{
		public Size MinSize => new Size(100, 30);

		public Size MaxSize => new Size(100, 30);

		Character[,] _memory = new Character[100, 30];

		public void Update(IControl control)
		{
			for (int x = 0; x < control.Size.Width; x++)
			{
				for (int y = 0; y < control.Size.Height; y++)
				{
					Update(control, Position.At(x, y));
				}
			}
		}

		public void Update(IControl control, in Position position)
		{
			var c = control[Position.At(position.X, position.Y)];

			if (c.Content != _memory[position.X, position.Y].Content)
			{
				_memory[position.X, position.Y] = c;

				Console.SetCursorPosition(position.X, position.Y);
				Console.Write($"\x1b[38;2;23;100;170m{c.Content}");
			}
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
			Console.CursorVisible = false;

			var testContext = new TestContext();

			var textBlock1 = new TextBlock
			{
				Text = "Heheszki1"
			};

			var textBlock2 = new TextBlock
			{
				Text = "Heheszki2"
			};

			var stackPanel = new VerticalStackPanel();
			stackPanel.Add(new Border { Content = new TextBlock { Text = "Counter" } });
			stackPanel.Add(textBlock1);
			stackPanel.Add(new Border { Content = new TextBlock { Text = "Stoper" } });
			stackPanel.Add(textBlock2);

			var border = new Border()
			{
				BorderPlacement = BorderPlacement.Left | BorderPlacement.Top | BorderPlacement.Right,
				Content = stackPanel
			};

			border.Context = testContext;
			testContext.Update(border);

			int frames = 0;
			var watch = new Stopwatch();
			watch.Start();

			for (int i = 0; ; i++)
			{
				textBlock1.Text = $"{i}";
				testContext.Update(border);

				if (watch.ElapsedMilliseconds > 1000)
				{
					textBlock2.Text = $"{frames}";
					Thread.Sleep(1000);
					watch.Restart();
					frames = 0;
					border.BorderPlacement ^= BorderPlacement.Left;
				}

				frames++;
			}
		}
	}
}
