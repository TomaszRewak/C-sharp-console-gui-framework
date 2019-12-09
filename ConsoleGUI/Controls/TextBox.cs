using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Controls
{
	public class TextBox : Control, IInputListener, IMouseListener
	{
		public event EventHandler Clicked;

		private string _text = string.Empty;
		public string Text
		{
			get => _text ?? string.Empty;
			set => Setter
				.Set(ref _text, value)
				.Then(Initialize);
		}

		private int _caretStart;
		public int CaretStart
		{
			get => Math.Min(Math.Max(_caretStart, 0), TextLength);
			set => Setter
				.Set(ref _caretStart, value)
				.Then(Redraw);
		}

		private int _caretEnd;
		public int CaretEnd
		{
			get => Math.Min(Math.Max(_caretEnd, CaretStart), TextLength);
			set => Setter
				.Set(ref _caretEnd, value)
				.Then(Redraw);
		}

		public int Caret
		{
			set
			{
				using(Freeze())
				{
					CaretStart = value;
					CaretEnd = value;
				}
			}
		}

		private bool _showCaret = true;
		public bool ShowCaret
		{
			get => _showCaret;
			set => Setter
				.Set(ref _showCaret, value)
				.Then(Redraw);
		}

		private int? _mouseDownPosition;
		private int? MouseDownPosition
		{
			get => _mouseDownPosition;
			set => Setter
				.Set(ref _mouseDownPosition, value)
				.Then(UpdateSelection);
		}

		private int? _mousePosition;
		private int? MousePosition
		{
			get => _mousePosition;
			set => Setter
				.Set(ref _mousePosition, value)
				.Then(UpdateSelection);
		}

		private int TextLength => Text?.Length ?? 0;
		private Size TextSize => new Size(TextLength, 1);
		private Size EditorSize => TextSize.Expand(1, 0);

		public override Cell this[Position position]
		{
			get
			{
				var content = EditorSize.Contains(position) && position.X < TextLength
					? Text[position.X]
					: (char?)null;

				var cell = new Cell(content).WithMouseListener(this, position);

				if (ShowCaret && position.X == CaretStart && position.X == CaretEnd)
					cell = cell.WithBackground(new Color(70, 70, 70));
				if (ShowCaret && position.X >= CaretStart && position.X < CaretEnd)
					cell = cell.WithBackground(Color.White).WithForeground(Color.Black);

				return cell;
			}
		}

		void IInputListener.OnInput(InputEvent inputEvent)
		{
			using (Freeze())
			{
				string newText = null;

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
					case ConsoleKey.UpArrow:
						CaretStart = CaretEnd = TextUtils.PreviousLine(Text, CaretStart);
						break;
					case ConsoleKey.DownArrow:
						CaretStart = CaretEnd = TextUtils.NextLine(Text, CaretEnd);
						break;
					case ConsoleKey.Delete when CaretStart != CaretEnd:
					case ConsoleKey.Backspace when CaretStart != CaretEnd:
						newText = $"{Text.Substring(0, CaretStart)}{Text.Substring(CaretEnd)}";
						CaretEnd = CaretStart;
						break;
					case ConsoleKey.Backspace when CaretStart > 0:
						newText = $"{Text.Substring(0, CaretStart - 1)}{Text.Substring(CaretStart)}";
						CaretStart = CaretEnd = CaretStart - 1;
						break;
					case ConsoleKey.Delete when CaretStart < TextLength:
						newText = $"{Text.Substring(0, CaretStart)}{Text.Substring(CaretStart + 1)}";
						break;
					case ConsoleKey key when char.IsControl(inputEvent.Key.KeyChar) && inputEvent.Key.Key != ConsoleKey.Enter:
						return;
					default:
						var character = inputEvent.Key.Key == ConsoleKey.Enter
							? '\n'
							: inputEvent.Key.KeyChar;
						newText = $"{Text.Substring(0, CaretStart)}{character}{Text.Substring(CaretEnd)}";
						CaretStart = CaretEnd = CaretStart + 1;
						break;
				}

				if (newText != null)
					Text = newText;

				inputEvent.Handled = true;
			}
		}

		protected override void Initialize()
		{
			using (Freeze())
			{
				Resize(EditorSize);
			}
		}

		private void UpdateSelection()
		{
			if (!MouseDownPosition.HasValue) return;
			if (!MousePosition.HasValue) return;

			CaretStart = Math.Min(MouseDownPosition.Value, MousePosition.Value);
			CaretEnd = Math.Max(MouseDownPosition.Value, MousePosition.Value);
		}

		void IMouseListener.OnMouseEnter()
		{ }

		void IMouseListener.OnMouseLeave()
		{
			MouseDownPosition = null;
			MousePosition = null;
		}

		void IMouseListener.OnMouseUp(Position position)
		{
			MousePosition = position.X;
			MouseDownPosition = null;
		}

		void IMouseListener.OnMouseDown(Position position)
		{
			MouseDownPosition = position.X;
			Clicked?.Invoke(this, EventArgs.Empty);
		}

		void IMouseListener.OnMouseMove(Position position)
		{
			MousePosition = position.X;
		}
	}
}
