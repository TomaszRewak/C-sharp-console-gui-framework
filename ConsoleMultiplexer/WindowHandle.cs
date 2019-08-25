using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public class WindowHandle : IDisposable
	{
		public Rect Rect { get; }
		public BorderPlacement Border { get; }

		public WindowHandle(in Rect rect, BorderPlacement border = BorderPlacement.None)
		{
			Rect = rect;
			Border = border;

			ConsoleManager.Register(this);
		}

		public void Dispose()
		{
			ConsoleManager.Unregister(this);
		}

		internal void Repaint(in Rect rect)
		{
			RepaintBorder(rect);
			RepaintContent(rect);
		}

		private void RepaintBorder(in Rect rect)
		{
			if (Border.HasFlag(BorderPlacement.Top))
				for (int i = 0; i < Rect.Width; i++)
					Paint(i, -1, '═');

			if (Border.HasFlag(BorderPlacement.Bottom))
				for (int i = 0; i < Rect.Width; i++)
					Paint(i, Rect.Height, '═');

			if (Border.HasFlag(BorderPlacement.Left))
				for (int i = 0; i < Rect.Height; i++)
					Paint(-1, i, '║');

			if (Border.HasFlag(BorderPlacement.Right))
				for (int i = 0; i < Rect.Height; i++)
					Paint(Rect.Width, i, '║');

			if (Border.HasFlag(BorderPlacement.Top | BorderPlacement.Left))
				Paint(-1, -1, '╔');

			if (Border.HasFlag(BorderPlacement.Top | BorderPlacement.Right))
				Paint(Rect.Width, -1, '╗');

			if (Border.HasFlag(BorderPlacement.Bottom | BorderPlacement.Left))
				Paint(-1, Rect.Height, '╚');

			if (Border.HasFlag(BorderPlacement.Bottom | BorderPlacement.Right))
				Paint(Rect.Width, Rect.Height, '╝');
		}

		private void RepaintContent(in Rect rect)
		{
			for (int x = 0; x < Rect.Width; x++)
				for (int y = 0; y < Rect.Height; y++)
					Paint(x, y, '▒');
		}

		private void RequestBuffer()
		{ }

		internal void Paint(int left, int top, char character)
		{
			Console.SetCursorPosition(Rect.Left + left, Rect.Top + top);
			Console.Write(character);
		}
	}
}
