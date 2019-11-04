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
			var textBlock = new TextBlock();
			var button = new Button { Content = new Margin { Offset = new Offset(4, 1, 4, 1), Content = new TextBlock { Text = "Button" } } };

			ConsoleManager.Content = new Background
			{
				Color = new Color(100, 0, 0),
				Content = new VerticalStackPanel
				{
					Children = new IControl[]
					{
						textBox,
						textBlock,
						new Box { Content = button }
					}
				}
			};

			var inputs = new IInputListener[]
			{
				textBox
			};

			while (true)
			{
				MouseHandler.ReadMouseEvents();
				ConsoleManager.ReadInput(inputs);

				textBlock.Text = $"Mouse position: ({ConsoleManager.MousePosition?.X}, {ConsoleManager.MousePosition?.Y})";

				Thread.Sleep(50);
			}
		}
	}
}
