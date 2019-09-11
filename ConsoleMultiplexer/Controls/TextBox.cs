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

		private int _caretStart;
		public int CaretStart
		{
			get => _caretStart;
			set => Setter
				.Set(ref _caretStart, value)
				.Then(Redraw);
		}

		private int _caretEnd;
		public int CaretEnd
		{
			get => _caretEnd;
			set => Setter
				.Set(ref _caretEnd, value)
				.Then(Redraw);
		}

		private Size TextSize => new Size(Text?.Length ?? 0, 1);

		public override Character this[Position position]
		{
			get
			{
				if (!TextSize.Contains(position)) return Character.Empty;

				if (position.X == CaretStart && position.X == CaretEnd)
					return new Character(Text[position.X], background: new Color(50, 50, 50));

				if (position.X >= CaretStart && position.X < CaretEnd)
					return new Character(Text[position.X], background: Color.White, foreground: Color.Black);

				return new Character(Text[position.X]);
			}
		}

		public void OnInput(InputEvent inputEvent)
		{
			using (Freeze())
			{
				switch (inputEvent.Key.Key)
				{
					case ConsoleKey.LeftArrow when inputEvent.Key.Modifiers.HasFlag(ConsoleModifiers.Control):
						CaretStart = Math.Max(0, CaretStart - 1);
						break;
					case ConsoleKey.LeftArrow:
						CaretStart = CaretEnd = Math.Max(0, CaretStart - 1);
						break;
					case ConsoleKey.RightArrow when inputEvent.Key.Modifiers.HasFlag(ConsoleModifiers.Control):
						CaretEnd = Math.Min(Text.Length, CaretEnd + 1);
						break;
					case ConsoleKey.RightArrow:
						CaretStart = CaretEnd = Math.Min(Text.Length, CaretEnd + 1);
						break;
					default:
						Text = Text ?? "";
						Text = $"{Text.Substring(0, CaretStart)}{inputEvent.Key.KeyChar}{Text.Substring(CaretEnd)}";
						CaretStart = CaretEnd = CaretStart + 1;
						break;
				}
			}

			inputEvent.Handled();
		}

		protected override void Resize()
		{
			Redraw(Size.Clip(MinSize, TextSize, MaxSize));
		}
	}
}
