using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using System;

namespace ConsoleGUI.Example
{
	class InputController : IInputListener
	{
		private readonly TextBox _textBox;
		private readonly LogPanel _logPanel;

		public InputController(TextBox textBox, LogPanel logPanel)
		{
			_textBox = textBox;
			_logPanel = logPanel;
		}

		public void OnInput(InputEvent inputEvent)
		{
			if (inputEvent.Key.Key != ConsoleKey.Enter) return;

			_logPanel.Add(_textBox.Text);

			_textBox.Text = string.Empty;
			inputEvent.Handled = true;
		}
	}
}
