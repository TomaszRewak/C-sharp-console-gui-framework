using ConsoleGUI.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Utils
{
	internal static class SafeConsole
	{
		public static void SetCursorPosition(int left, int top)
		{
			try
			{
				Console.SetCursorPosition(left, top);
			}
			catch (Exception)
			{ }
		}

		public static void SetWindowPosition(int left, int top)
		{
			try
			{
				Console.SetWindowPosition(left, top);
			}
			catch (Exception)
			{ }
		}

		public static void SetWindowSize(int width, int height)
		{
			try
			{
				Console.SetWindowSize(width, height);
			}
			catch (Exception)
			{ }
		}

		public static void SetBufferSize(int width, int height)
		{
			try
			{
				Console.SetBufferSize(width, height);
			}
			catch (Exception)
			{ }
		}

		public static void WriteOrThrow(int left, int top, string content)
		{
			try
			{
				Console.SetCursorPosition(left, top);
				Console.Write(content);
			}
			catch (Exception)
			{
				throw new SafeConsoleException();
			}
		}

		public static void WriteOrThrow(int left, int top, ConsoleColor background, ConsoleColor foreground, char content)
		{
			try
			{
				Console.SetCursorPosition(left, top);
				Console.BackgroundColor = background;
				Console.ForegroundColor = foreground;
				Console.Write(content);
			}
			catch (Exception)
			{
				throw new SafeConsoleException();
			}
		}

		public static void SetUtf8()
		{
			try
			{
				Console.OutputEncoding = Encoding.UTF8;
			}
			catch (Exception)
			{ }
		}

		public static void HideCursor()
		{
			try
			{
				Console.CursorVisible = false;
			}
			catch (Exception)
			{ }
		}

		public static void Clear()
		{
			try
			{
				Console.Clear();
			}
			catch (Exception)
			{ }
		}
	}
}
