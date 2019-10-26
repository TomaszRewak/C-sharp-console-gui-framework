using System;
using System.Collections.Generic;
using System.Text;
using ConsoleMultiplexer.Controls;
using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Input;
using ConsoleMultiplexer.Space;

namespace ConsoleMultiplexer.Example
{
	internal class TabPanel : IControl, IInputListener
	{
		private class Tab
		{
			private readonly Background hederBackground;

			public IControl Header { get; }
			public IControl Content { get; }

			public Tab(string name, IControl content)
			{
				hederBackground = new Background
				{
					Content = new Margin
					{
						Offset = new Offset(1, 0, 1, 0),
						Content = new TextBlock { Text = name }
					}
				};

				Header = new Margin
				{
					Offset = new Offset(0, 0, 1, 0),
					Content = hederBackground
				};
				Content = content;

				MarkAsInactive();
			}

			public void MarkAsActive() => hederBackground.Color = new Color(25, 54, 65);
			public void MarkAsInactive() => hederBackground.Color = new Color(65, 24, 25);
		}

		private readonly List<Tab> tabs = new List<Tab>();
		private readonly DockPanel wrapper;
		private readonly HorizontalStackPanel tabsPanel;

		private Tab currentTab;

		public TabPanel()
		{
			tabsPanel = new HorizontalStackPanel();

			wrapper = new DockPanel
			{
				Placement = DockPanel.DockedContorlPlacement.Top,
				DockedControl = new Background
				{
					Color = new Color(25, 25, 52),
					Content = new Boundary
					{
						MinHeight = 1,
						MaxHeight = 1,
						Content = tabsPanel
					}
				}
			};
		}

		public void AddTab(string name, IControl content)
		{
			var newTab = new Tab(name, content);
			tabs.Add(newTab);
			tabsPanel.Add(newTab.Header);
			if (tabs.Count == 1)
				SelectTab(0);
		}

		public void SelectTab(int tab)
		{
			currentTab?.MarkAsInactive();
			currentTab = tabs[tab];
			currentTab.MarkAsActive();
			wrapper.FillingControl = currentTab.Content;
		}

		public void OnInput(InputEvent inputEvent)
		{
				if (inputEvent.Key.Key != ConsoleKey.Tab) return;

			SelectTab((tabs.IndexOf(currentTab) + 1) % tabs.Count);
			inputEvent.Handled = true;
		}

		public Character this[Position position] => wrapper[position];
		public Size Size => wrapper.Size;
		public IDrawingContext Context
		{
			get => (wrapper as IControl).Context;
			set => (wrapper as IControl).Context = value;
		}
	}
}
