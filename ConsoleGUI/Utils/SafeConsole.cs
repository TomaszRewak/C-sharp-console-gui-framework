using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Utils
{
	internal static class SafeConsole
	{
		public static void SetWindowPosition(int left, int top)
		{
			try
			{
				Console.SetWindowPosition(left, top);
			}
			catch (PlatformNotSupportedException)
			{ }
		}

		public static void SetWindowSize(int width, int height)
		{
			try
			{
				Console.SetWindowSize(width, height);
			}
			catch (PlatformNotSupportedException)
			{ }
		}

		public static void SetBufferSize(int width, int height)
		{
			try
			{
				Console.SetBufferSize(width, height);
			}
			catch (PlatformNotSupportedException)
			{ }
		}

		public static void SetUtf8()
		{
			try
			{
				Console.OutputEncoding = Encoding.UTF8;
			}
			catch (PlatformNotSupportedException)
			{ }
		}
	}
}
