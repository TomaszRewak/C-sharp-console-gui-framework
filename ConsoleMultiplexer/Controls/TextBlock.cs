using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class TextBlock : IControl
	{
		private IDrawingContext _context;

		private string _text;
		public string Text
		{
			get => _text;
			set
			{
				if (_text == value) return;
				_text = value;

				PrintText();
			}
		}

		public void Draw(IDrawingContext context)
		{
			_context = context;

			PrintText();
		}

		private void PrintText()
		{
			_context.Clear();

			Position end = Position.Begin;

			foreach (var character in Text)
			{
				switch(character)
				{
					case '\n':
						end = end.NextLine;
						break;
					default:
						_context.Set(end, new Character(character));
						end = end.Next;
						break;
				}
			}

			_context.Flush();
		}
	}
}
