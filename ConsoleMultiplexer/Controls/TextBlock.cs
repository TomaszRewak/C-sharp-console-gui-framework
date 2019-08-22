using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class TextBlock : IControl
	{
		public IDrawingContext Context { get; set; }

		private string _text;
		public string Text
		{
			get => _text;
			set
			{
				if (_text == value) return;
				_text = value;

				Draw();
			}
		}

		public void Draw()
		{
			if (Context == null) return;

			Context.Clear();
			PrintText();
			Context.Flush();
		}

		private void PrintText()
		{
			Position end = Position.Begin;

			foreach (var character in Text)
			{
				switch (character)
				{
					case '\n':
						end = end.NextLine;
						break;
					default:
						Context.Set(end, new Character(character));
						end = end.Next;
						break;
				}
			}
		}
	}
}
