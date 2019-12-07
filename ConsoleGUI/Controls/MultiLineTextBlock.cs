using ConsoleGUI.UserDefined;
using ConsoleGUI.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGUI.Controls
{
	public class MultiLineTextBlock : SimpleControl
	{
		private readonly VerticalStackPanel _stackPanel = new VerticalStackPanel();
		private readonly List<WrapPanel> _wrapPanels = new List<WrapPanel>();

		public MultiLineTextBlock()
		{
			Content = _stackPanel;
		}

		private string _text;
		public string Text
		{
			get => _text;
			set => Setter
				.Set(ref _text, value)
				.Then(Initialize);
		}

		public void Initialize()
		{
			var lines = Text.Split('\n');

			while (_wrapPanels.Count < lines.Length)
			{
				var newWrapPanel = new WrapPanel { Content = new TextBlock() };
				_wrapPanels.Add(newWrapPanel);
				_stackPanel.Add(newWrapPanel);
			}

			while (_wrapPanels.Count > lines.Length)
			{
				var lastWrapPanel = _wrapPanels.Last();
				_wrapPanels.RemoveAt(_wrapPanels.Count - 1);
				_stackPanel.Remove(lastWrapPanel);
			}

			for (int i = 0; i < lines.Length; i++)
			{
				var textBlock = _wrapPanels[i].Content as TextBlock;
				textBlock.Text = lines[i];
			}
		}
	}
}
