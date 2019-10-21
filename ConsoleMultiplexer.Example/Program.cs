using ConsoleMultiplexer.Controls;
using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Input;
using ConsoleMultiplexer.Space;
using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleMultiplexer.Example
{
	class Person
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public DateTime BirthDate { get; set; }

		public Person(string name, string surname, DateTime birthDate)
		{
			Name = name;
			Surname = surname;
			BirthDate = birthDate;
		}
	}

	class Program
	{
		static void Main()
		{
			var dockPanel = new DockPanel
			{
				Placement = DockPanel.DockedContorlPlacement.Top,
				DockedControl = new DockPanel
				{
					Placement = DockPanel.DockedContorlPlacement.Right,
					DockedControl = new TextBlock { Text = "Clock" },
					FillingControl = new Background
					{
						Fill = new Character(' ', background: new Color(100, 0, 0)),
						Content = new Box
						{
							Content = new TextBlock { Text = "Center" },
							HorizontalContentPlacement = Box.HorizontalPlacement.Center
						}
					}
				}
			};

			ConsoleManager.Content = dockPanel;

			while (true)
			{
				Thread.Sleep(20);
			}
		}

		static void Main2()
		{
			var textBlock1 = new TextBlock
			{
				Text = "Heheszki1",
				Color = new Color(87, 200, 157)
			};

			var border1 = new Border
			{
				BorderPlacement = BorderPlacement.All ^ BorderPlacement.Bottom,
				Content = textBlock1
			};

			var textBlock2 = new TextBlock
			{
				Text = "Heheszki2",
				Color = new Color(157, 42, 157)
			};

			var border2 = new Border
			{
				BorderPlacement = BorderPlacement.Left | BorderPlacement.Right,
				Content = new HorizontalAlignment
				{
					Content = textBlock2
				},
				BorderStyle = BorderStyle.Double.WithColor(new Color(200, 0, 0))
			};

			var textBox = new TextBox();

			var stackPanel1 = new VerticalStackPanel();
			stackPanel1.Add(new TextBlock { Text = "Test1" });
			stackPanel1.Add(new TextBlock { Text = "Test2" });
			stackPanel1.Add(border1);
			stackPanel1.Add(border2);
			stackPanel1.Add(new WrapPanel
			{
				Children = new IControl[] {
					new TextBlock
					{
						Color = Color.LightBlue,
						Text = @"D:\test> "
					},
					textBox
				}
			});

			var border3 = new Border
			{
				BorderPlacement = BorderPlacement.All,
				Content = stackPanel1,
				BorderStyle = BorderStyle.Double.WithColor(new Color(100, 100, 50))
			};

			var border4 = new Border
			{
				Content = new Background
				{
					Fill = new Character('.', new Color(123, 54, 34), new Color(65, 25, 235)),
					Content = new HorizontalAlignment
					{
						Content = new Style
						{
							Foreground = new Color(64, 132, 54),
							Background = new Color(65, 31, 64),
							Content = new TextBlock
							{
								Text = "Test"
							}
						}
					}
				}
			};

			var scrollPanel = new VerticalScrollPanel
			{
				Top = 0,
				Content = new VerticalStackPanel
				{
					Children = new IControl[] {
							new TextBlock {Text = "Test 1"},
							new TextBlock {Text = "Test 2"},
							new TextBlock {Text = "Test 3"},
							new TextBlock {Text = "Test 4"},
							new TextBlock {Text = "Test 5"},
							new HorizontalSeparator(),
							new TextBlock {Text = "Test 6"},
							new TextBlock {Text = "Test 7"},
							new TextBlock {Text = "Test 8"},
							new TextBlock {Text = "Test 9"},
							new TextBlock {Text = "Test 10"},
							new TextBlock {Text = "Test 11"},
							new TextBlock {Text = "Test 12"},
							new HorizontalSeparator(),
							new TextBlock {Text = "Test 13"},
							new TextBlock {Text = "Test 14"},
							new TextBlock {Text = "Test 15"}
						}
				}
			};

			var border5 = new Border
			{
				Content = scrollPanel,
				BorderStyle = BorderStyle.Single
			};

			var textBlock4 = new TextBlock
			{
				Text = "Heheszki2",
				Color = new Color(157, 42, 157)
			};

			var border6 = new Border
			{

				Content = new Margin
				{
					Offset = new Offset(1, 3, 0, 0),
					Content = new WrapPanel
					{
						Children = new[] {
							new TextBlock {Text = "Test 1"},
							new TextBlock {Text = "Test 2"},
							new TextBlock {Text = "Test 3"},
							textBlock4,
							new TextBlock {Text = "Test 4"},
							new TextBlock {Text = "Test 5"},
							new TextBlock {Text = "Test 6"},
							new TextBlock {Text = "Test 7"},
						}
					}
				}
			};

			var grid = new Grid
			{
				Columns = new[]
					{
						new Grid.ColumnDefinition(2),
						new Grid.ColumnDefinition(3),
						new Grid.ColumnDefinition(4)
					},
				Rows = new[]
					{
						new Grid.RowDefinition(3),
						new Grid.RowDefinition(4),
						new Grid.RowDefinition(5),
					}
			};

			for (int i = 0; i < 9; i++)
				grid.AddChild(i / 3, i % 3, new Background { Fill = new Character(null, background: i % 2 == 0 ? new Color(255, 255, 255) : new Color(200, 100, 50)) });
			grid.AddChild(1, 1, new WrapPanel { Children = new[] { new TextBlock { Text = "Middle one" } } });

			var border7 = new Border
			{
				Content = grid
			};

			var border8 = new Border
			{
				Content = new DataGrid<Person>
				{
					Columns = new[]
					{
						new DataGrid<Person>.ColumnDefinition("Name", 10, p=> p.Name),
						new DataGrid<Person>.ColumnDefinition("Surname", 10, p=> p.Surname),
						new DataGrid<Person>.ColumnDefinition("Birth date", 10, p => p.BirthDate.ToShortDateString(), background: p => DateTime.Now.Year < p.BirthDate.Year + 18 ? new Color(255, 0,0) : new Color(0, 0, 255))
					},
					Data = new[]
					{
						new Person("John", "Connor", new DateTime(1985, 2, 28)),
						new Person("Ellen", "Ripley", new DateTime(2092, 1, 1)),
						new Person("Jan", "Kowalski", new DateTime(1990, 4, 10)),
					}
				}
			};

			var border9 = new Border
			{
				Content = new DockPanel
				{
					Placement = DockPanel.DockedContorlPlacement.Botton,
					DockedControl = new WrapPanel { Children = new[] { new TextBlock { Text = "Docked text" } } },
					FillingControl = new Background { Fill = new Character(null, background: new Color(100, 110, 120)) }
				}
			};

			var canvas = new Canvas();
			canvas.Add(border3, new Rect(20, 10, 70, 20));
			canvas.Add(border4, new Rect(40, 5, 60, 10));
			canvas.Add(border5, new Rect(35, 16, 40, 10));
			canvas.Add(border6, new Rect(5, 5, 10, 15));
			canvas.Add(border7, new Rect(80, 20, 20, 15));
			canvas.Add(border8, new Rect(20, 30, 100, 10));
			canvas.Add(border9, new Rect(5, 25, 7, 15));

			ConsoleManager.Content = canvas;

			int frames = 0;
			var watch = new Stopwatch();
			watch.Start();

			var inputs = new IInputListener[]
			{
				textBox,
				scrollPanel
			};

			for (int i = 0; ; i++)
			{
				textBlock1.Text = $"{i}";

				if (watch.ElapsedMilliseconds > 1000)
				{
					textBlock2.Text = $"{frames}";
					watch.Restart();
					frames = 0;
					border3.BorderPlacement ^= BorderPlacement.Left;
					textBlock4.Text = new Random().Next().ToString();
				}

				ConsoleManager.ReadInput(inputs);

				frames++;
			}
		}
	}
}
