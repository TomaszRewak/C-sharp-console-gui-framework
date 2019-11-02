using ConsoleGUI.Input;
using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Data
{
	public readonly struct Cell
	{
		public readonly Character Character;
		public readonly MouseListener? MouseListener;

		public Cell(in Character character)
		{
			Character = character;
			MouseListener = null;
		}

		public Cell(in Character character, in MouseListener? mouseListener)
		{
			Character = character;
			MouseListener = mouseListener;
		}

		public char? Content => Character.Content;
		public Color? Foreground => Character.Foreground;
		public Color? Background => Character.Background;

		public Cell WithContent(char? content) => new Cell(Character.WithContent(content), MouseListener);
		public Cell WithForeground(in Color? foreground) => new Cell(Character.WithForeground(foreground), MouseListener);
		public Cell WithBackground(in Color? background) => new Cell(Character.WithBackground(background), MouseListener);
		public Cell WithMouseListener(IMouseListener listener, in Position position) => new Cell(Character, new MouseListener(listener, position));

		public static implicit operator Cell(in Character character) => new Cell(character);
	}
}
