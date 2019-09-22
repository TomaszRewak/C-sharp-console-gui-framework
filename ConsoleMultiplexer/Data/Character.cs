using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Data
{
	public struct Character
	{
		public char? Content { get; }

		public Color? Foreground { get; }
		public Color? Background { get; }

		public bool IsNewLine => Content == '\n';
		public bool IsEmpty => !Content.HasValue;

		public Character(char? content, Color? foreground = null, Color? background = null)
		{
			Content = content;
			Foreground = foreground;
			Background = background;
		}

		public static Character Empty => new Character();
	}
}
