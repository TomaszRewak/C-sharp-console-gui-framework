using System;
using System.Collections.Generic;
using System.Text;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Space;

namespace ConsoleGUI.Example
{
	internal class LogPanel : IControl
	{
		private readonly VerticalStackPanel _stackPanel;

		public LogPanel()
		{
			_stackPanel = new VerticalStackPanel();
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

		public Character this[Position position] => _stackPanel[position];
		public Size Size => _stackPanel.Size;
		public IDrawingContext Context
		{
			get => (_stackPanel as IControl).Context;
			set => (_stackPanel as IControl).Context = value;
		}
	}
}
