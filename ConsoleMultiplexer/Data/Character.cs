using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Data
{
	public readonly struct Character
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

		public Character WithForeground(in Color? foreground) => new Character(Content, foreground, Background);

		public static Character Empty => new Character();

		public static bool operator==(in Character lhs, in Character rhs)
		{
			return lhs.Content == rhs.Content &&
				   lhs.Foreground == rhs.Foreground &&
				   lhs.Background == rhs.Background;
		}

		public static bool operator !=(in Character lhs, in Character rhs) => !(lhs == rhs);

		public override bool Equals(object obj)
		{
			return obj is Character character && this == character;
		}
	}
}
