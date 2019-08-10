using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public class WindowHandle : IDisposable
	{
		public ScreenRect Rect { get; }
		public WindowBorder Border { get; }

		public WindowHandle(in ScreenRect rect, WindowBorder border = WindowBorder.None)
		{
			Rect = rect;
			Border = border;

			ConsoleManager.Register(this);
		}

		public void Dispose()
		{
			ConsoleManager.Unregister(this);
		}

		internal void Repaint(in ScreenRect rect)
		{
			RepaintBorder(rect);
			RepaintContent(rect);
		}

		private void RepaintBorder(in ScreenRect rect)
		{
			if (Border.HasFlag(WindowBorder.Top))
				for (int i = 0; i < Rect.Width; i++)
					Paint(i, -1, '═');

			if (Border.HasFlag(WindowBorder.Bottom))
				for (int i = 0; i < Rect.Width; i++)
					Paint(i, Rect.Height, '═');

			if (Border.HasFlag(WindowBorder.Left))
				for (int i = 0; i < Rect.Height; i++)
					Paint(-1, i, '║');

			if (Border.HasFlag(WindowBorder.Right))
				for (int i = 0; i < Rect.Height; i++)
					Paint(Rect.Width, i, '║');

			if (Border.HasFlag(WindowBorder.Top | WindowBorder.Left))
				Paint(-1, -1, '╔');

			if (Border.HasFlag(WindowBorder.Top | WindowBorder.Right))
				Paint(Rect.Width, -1, '╗');

			if (Border.HasFlag(WindowBorder.Bottom | WindowBorder.Left))
				Paint(-1, Rect.Height, '╚');

			if (Border.HasFlag(WindowBorder.Bottom | WindowBorder.Right))
				Paint(Rect.Width, Rect.Height, '╝');
		}

		private void RepaintContent(in ScreenRect rect)
		{
			for (int x = 0; x < Rect.Width; x++)
				for (int y = 0; y < Rect.Height; y++)
					Paint(x, y, '▒');
		}

		internal void Paint(int left, int top, char character)
		{
			Console.SetCursorPosition(Rect.Left + left, Rect.Top + top);
			Console.Write(character);
		}
	}
}
