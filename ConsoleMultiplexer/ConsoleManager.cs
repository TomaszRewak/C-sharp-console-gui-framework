using System;
using System.Collections.Generic;

namespace ConsoleMultiplexer
{
	internal class ConsoleManager
	{
		private static readonly List<WindowHandle> _windows = new List<WindowHandle>();

		internal static void Register(WindowHandle windowHandle)
		{
			windowHandle.Repaint(windowHandle.Rect);
		}

		internal static void Unregister(WindowHandle windowHandle)
		{

		}
	}
}
