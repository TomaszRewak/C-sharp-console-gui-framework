using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class TextBox : Control, IInputListener
	{
		private string _text;
		public string Text
		{
			get => _text;
			set => Setter
				.Set(ref _text, value)
				.Then(Resize);
		}

		private Size TextSize => new Size(Text?.Length ?? 0, 1);

		public override Character this[Position position]
		{
			get
			{
				if (!TextSize.Contains(position)) return Character.Empty;

				return new Character(Text[position.X]);
			}
		}

		public void OnInput(InputEvent inputEvent)
		{
			Text += inputEvent.Key.KeyChar;

			inputEvent.Handled();
		}

		protected override void Resize()
		{
			Redraw(Size.Clip(MinSize, TextSize, MaxSize));
		}
	}
}
