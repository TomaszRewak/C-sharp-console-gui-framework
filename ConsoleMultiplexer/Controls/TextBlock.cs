using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class TextBlock : IControl
	{
		private UIContext _context = UIContext.Empty;

		private string _text;
		public string Text
		{
			get => _text;
			set => Setter
				.Set(ref _text, value)
				.Then(Draw);
		}

		public void Draw(UIContext context)
		{
			_context = context;
			Draw();
		}

		private void Draw()
		{
			_context.Clear();

			Position end = Position.Begin;
			foreach (var character in Text)
			{
				switch (character)
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
