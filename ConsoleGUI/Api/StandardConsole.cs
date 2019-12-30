using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Api
{
	public class StandardConsole : IConsole
	{
		public Size Size
		{
			get => new Size(Console.WindowWidth, Console.WindowHeight);
			set
			{
				Console.SetCursorPosition(0, 0);
				SafeConsole.SetWindowPosition(0, 0);
				if (!(Size <= value)) SafeConsole.SetWindowSize(1, 1);
				SafeConsole.SetBufferSize(value.Width, value.Height);
				if (Size != value) SafeConsole.SetWindowSize(value.Width, value.Height);
				Initialize();
			}
		}

		public bool KeyAvailable => Console.KeyAvailable;

		public virtual void Initialize()
		{
			SafeConsole.SetUtf8();
			SafeConsole.HideCursor();
			SafeConsole.Clear();
		}

		public virtual void OnRefresh()
		{
			SafeConsole.HideCursor();
		}

		public virtual void Write(Position position, in Character character)
		{
			var content = character.Content ?? ' ';
			var foreground = character.Foreground ?? Color.White;
			var background = character.Background ?? Color.Black;

			if (content == '\n') content = ' ';

			Console.SetCursorPosition(position.X, position.Y);
			Console.Write($"\x1b[38;2;{foreground.Red};{foreground.Green};{foreground.Blue}m\x1b[48;2;{background.Red};{background.Green};{background.Blue}m{content}");
		}

		public ConsoleKeyInfo ReadKey()
		{
			return Console.ReadKey(true);
		}
	}
}
