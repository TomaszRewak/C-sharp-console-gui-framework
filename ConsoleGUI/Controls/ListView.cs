using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.UserDefined;

namespace ConsoleGUI.Controls
{
	public class ListView : SimpleControl, IInputListener
	{
		public Color TextColor { get; set; } = Color.White;
		public Color SelectedTextColor { get; set; } = Color.Black;

		private int selectedIndex = -1;
		private int totalCount = -1;

		public ListView()
		{
			Content = new VerticalScrollPanel()
			{
				Content = new VerticalStackPanel()
			};
		}

		public string SelectedText
		{
			get
			{
				try
                {
					var selectedItem = ((Content as VerticalScrollPanel).Content as VerticalStackPanel).Children.ElementAt(selectedIndex);
					var current = selectedItem as TextBlock;
					return current.Text;
				}
				catch
                {
					return string.Empty;
				}
			}
		}

		public List<string> Items
        {
			set
            {
				totalCount = value.Count;
				selectedIndex = value.Any() ? 0 : -1;

				var items = new List<TextBlock>();
				for (int x = 0; x < value.Count; x++)
                {
					items.Add(new TextBlock() { Text = value[x], Color = x == selectedIndex ? SelectedTextColor : TextColor });
                }

				((Content as VerticalScrollPanel).Content as VerticalStackPanel).Children = items;
			}
        }

		public void OnInput(InputEvent inputEvent)
		{
			if (inputEvent.Key.Key == ConsoleKey.UpArrow)
            {
				if (selectedIndex > 0)
				{
					Update(selectedIndex, selectedIndex - 1);
					selectedIndex--;
				}
			} 
			else if (inputEvent.Key.Key == ConsoleKey.DownArrow)
            {
				if (selectedIndex < totalCount - 1)
				{
					Update(selectedIndex, selectedIndex + 1);
					selectedIndex++;
				}
			}

			// pass through event to ensure scrolling works
			var input = Content as IInputListener;
			input.OnInput(inputEvent);
		}

		private void Update(int oldRow, int newRow)
        {
			var oldElement = ((Content as VerticalScrollPanel).Content as VerticalStackPanel).Children.ElementAt(oldRow);
			var newElement = ((Content as VerticalScrollPanel).Content as VerticalStackPanel).Children.ElementAt(newRow);

			var textBlockOld = oldElement as TextBlock;
			var textBlockNew = newElement as TextBlock;

			textBlockNew.Color = SelectedTextColor;
			textBlockOld.Color = TextColor;	
		}
	}
}
