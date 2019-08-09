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
				for (int i = 1; i < Rect.Width - 1; i++)
					Paint(Rect.Left + i, Rect.Top, '═');

			if (Border.HasFlag(WindowBorder.Top))
				for (int i = 1; i < Rect.Width - 1; i++)
					Paint(Rect.Left + i, Rect.Top + Rect.Height - 1, '═');

			if (Border.HasFlag(WindowBorder.Top))
				for (int i = 1; i < Rect.Height - 1; i++)
					Paint(Rect.Left, Rect.Top + i, '║');

			if (Border.HasFlag(WindowBorder.Top))
				for (int i = 1; i < Rect.Height - 1; i++)
					Paint(Rect.Left + Rect.Width - 1, Rect.Top + i, '║');

		}

		private void RepaintContent(in ScreenRect rect)
		{
			var innterRect = Rect.Crop(Border);

			for (int x = 0; x < innterRect.Width; x++)
				for (int y = 0; y < innterRect.Height; y++)
					Paint(innterRect.Left + x, innterRect.Top + y, '▒');
		}

		internal void Paint(int left, int top, char character)
		{
			Console.SetCursorPosition(left, top);
			Console.Write(character);
		}
	}
}
