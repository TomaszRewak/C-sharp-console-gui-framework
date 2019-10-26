using System;
using System.Collections.Generic;
using System.Text;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.UserDefined;

namespace ConsoleGUI.Example
{
	internal class LogPanel : SimpleControl
	{
		private readonly VerticalStackPanel _stackPanel;

		public LogPanel()
		{
			_stackPanel = new VerticalStackPanel();

			Content = _stackPanel;
		}

		public void Add(string message)
		{
			_stackPanel.Add(new WrapPanel
			{
				Children = new IControl[]
				{
					new TextBlock {Text = $"[{DateTime.Now.ToLongTimeString()}] ", Color = new Color(200, 20, 20)},
					new TextBlock {Text = message}
				}
			});
		}
	}
}
