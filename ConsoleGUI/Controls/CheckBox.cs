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
	public class CheckBox : Control, IInputListener, IMouseListener
	{
		private bool _mouseDown;

		public event EventHandler<bool> ValueChanged;

		private bool _value;
		public bool Value
		{
			get => _value;
			set => Setter
				.Set(ref _value, value)
				.Then(Redraw);
		}

		private Character _trueCharacter = new Character('☑');
		public Character TrueCharacter
		{
			get => _trueCharacter;
			set => Setter
				.Set(ref _trueCharacter, value)
				.Then(Redraw);
		}

		private Character _falseCharacter = new Character('☐');
		public Character FalseCharacter
		{
			get => _falseCharacter;
			set => Setter
				.Set(ref _falseCharacter, value)
				.Then(Redraw);
		}

		public override Cell this[Position position]
		{
			get
			{
				if (position != Position.Begin) return Character.Empty;

				var character = Value
					? TrueCharacter
					: FalseCharacter;

				return new Cell(character).WithMouseListener(this, position);
			}
		}

		protected override void Initialize()
		{
			Resize(new Size(1, 1));
		}

		void IMouseListener.OnMouseEnter()
		{ }

		void IMouseListener.OnMouseLeave()
		{
			_mouseDown = false;
		}

		void IMouseListener.OnMouseUp(Position position)
		{
			if (_mouseDown)
			{
				_mouseDown = false;
				Value = !Value;
				ValueChanged?.Invoke(this, Value);
			}
		}

		void IMouseListener.OnMouseDown(Position position)
		{
			_mouseDown = true;
		}

		void IMouseListener.OnMouseMove(Position position)
		{ }

		void IInputListener.OnInput(InputEvent inputEvent)
		{
			if (inputEvent.Key.Key == ConsoleKey.Spacebar)
			{
				Value = !Value;
				inputEvent.Handled = true;
			}
		}
	}
}
