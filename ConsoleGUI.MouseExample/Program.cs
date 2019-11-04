using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleGUI.MouseExample
{
	class InputController
	{
		private readonly TextBox _textBox1;
		private readonly TextBox _textBox2;
		private readonly Button _button;

		private TextBox _selectedTextBox;

		public InputController(TextBox textBox1, TextBox textBox2, Button button)
		{
			_textBox1 = textBox1;
			_textBox2 = textBox2;
			_button = button;

			_textBox1.ShowCaret = false;
			_textBox2.ShowCaret = false;

			_textBox1.Clicked += TextBoxClicked;
			_textBox2.Clicked += TextBoxClicked;
			_button.Clicked += ButtonClicked;
		}

		public void ReadInput()
		{
			MouseHandler.ReadMouseEvents();
			ConsoleManager.ReadInput(new[] { _selectedTextBox });
		}

		private void TextBoxClicked(object sender, EventArgs e)
		{
			if (_selectedTextBox != null) _selectedTextBox.ShowCaret = false;
			_selectedTextBox = sender as TextBox;
			if (_selectedTextBox != null) _selectedTextBox.ShowCaret = true;
		}

		private void ButtonClicked(object sender, EventArgs e)
		{
			_textBox1.Text = "";
			_textBox2.Text = "";
		}
	}

	class Program
	{
		static void Main()
		{
			MouseHandler.Initialize();

			ConsoleManager.Setup();
			ConsoleManager.CompatibilityMode = true;
			ConsoleManager.DontPrintTheLastCharacter = true;
			ConsoleManager.Resize(new Size(150, 40));

			var textBox1 = new TextBox { Text = "Hello world" };
			var textBox2 = new TextBox { Text = "Test" };
			var textBlock = new TextBlock();
			var button = new Button { Content = new Margin { Offset = new Offset(4, 1, 4, 1), Content = new TextBlock { Text = "Button" } } };

			ConsoleManager.Content = new Background
			{
				Color = new Color(100, 0, 0),
				Content = new Margin
				{
					Offset = new Offset(5, 2, 5, 2),
					Content = new VerticalStackPanel
					{
						Children = new IControl[]
						{
							textBlock,
							new HorizontalSeparator(),
							new TextBlock { Text = "Simple text box" },
							new Background{
								Color = new Color(100, 100, 0),
								Content = textBox1
							},
							new HorizontalSeparator(),
							new TextBlock { Text = "Wrapped text box" },
							new Boundary
							{
								Width = 10,
								Content = new Background
								{
									Color = new Color(0, 100, 0),
									Content = new WrapPanel { Content = new Boundary{ MinWidth = 10, Content = textBox2 } }
								}
							},
							new HorizontalSeparator(),
							new Box { Content = button }
						}
					}
				}
			};

			var inputController = new InputController(textBox1, textBox2, button);

			while (true)
			{
				inputController.ReadInput();

				textBlock.Text = $"Mouse position: ({ConsoleManager.MousePosition?.X}, {ConsoleManager.MousePosition?.Y})";

				Thread.Sleep(50);
			}
		}
	}
}
