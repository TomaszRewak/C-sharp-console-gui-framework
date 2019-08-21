using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public struct Character
	{
		public char Content { get; }

		public ConsoleColor? Foreground { get; }
		public ConsoleColor? Background { get; }

		public bool IsNewLine => Content == '\n';

		public Character(char content, ConsoleColor? foreground = null, ConsoleColor? background = null)
		{
			Content = content;
			Foreground = foreground;
			Background = background;
		}

		public static Character Empty => new Character(' ');
	}
}
