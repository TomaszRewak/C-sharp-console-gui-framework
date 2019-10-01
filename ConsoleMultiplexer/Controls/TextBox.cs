using ConsoleMultiplexer.Common;
using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Input;
using ConsoleMultiplexer.Space;
using ConsoleMultiplexer.Utils;
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
				.Then(Initialize);
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

		private int TextLength => Text?.Length ?? 0;
		private Size TextSize => new Size(TextLength, 1);

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

		void IInputListener.OnInput(InputEvent inputEvent)
		{
			using (Freeze())
			{
				CaretStart = Math.Max(CaretStart, 0);
				CaretEnd = Math.Min(CaretEnd, TextLength);

				switch (inputEvent.Key.Key)
				{
					case ConsoleKey.UpArrow:
					case ConsoleKey.DownArrow:
						return;
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
					case ConsoleKey.Backspace when CaretStart != CaretEnd:
						Text = $"{Text.Substring(0, CaretStart)}{Text.Substring(CaretEnd)}";
						CaretEnd = CaretStart;
						break;
					case ConsoleKey.Backspace when CaretStart > 0:
						Text = $"{Text.Substring(0, CaretStart - 1)}{Text.Substring(CaretStart)}";
						CaretStart = CaretEnd = CaretStart - 1;
						break;
					default:
						Text = Text ?? "";
						Text = $"{Text.Substring(0, CaretStart)}{inputEvent.Key.KeyChar}{Text.Substring(CaretEnd)}";
						CaretStart = CaretEnd = CaretStart + 1;
						break;
				}
			}

			inputEvent.Handled = true;
		}

		protected override void Initialize()
		{
			Resize(Size.Clip(MinSize, TextSize, MaxSize));
		}
	}
}
