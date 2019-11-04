using ConsoleGUI.Controls;
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
	class Program
	{
		static void Main()
		{
			MouseHandler.Initialize();

			ConsoleManager.Setup();
			ConsoleManager.CompatibilityMode = true;
			ConsoleManager.DontPrintTheLastCharacter = true;
			ConsoleManager.Resize(new Size(150, 40));

			var textBox = new TextBox { Text = "Hello world" };
			var textBlock = new TextBlock { Text = "Hello world" };
			var button = new Button { Content = new Margin { Offset = new Offset(4, 1, 4, 1), Content = new TextBlock { Text = "Button" } } };

			ConsoleManager.Content = new VerticalStackPanel
			{
				Children = new IControl[]
				{
					textBox,
					textBlock,
					new Box { Content = button }
				}
			};

			var inputs = new IInputListener[]
			{
				textBox
			};

			while (true)
			{
				MouseHandler.Read();
				ConsoleManager.ReadInput(inputs);
				ConsoleManager.OnMouseMove(MouseHandler.MousePosition);

				textBlock.Text = $"({MouseHandler.MousePosition.X}, {MouseHandler.MousePosition.Y})";

				Thread.Sleep(50);
			}
		}
	}
}
