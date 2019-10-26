using ConsoleMultiplexer.Controls;
using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Input;
using ConsoleMultiplexer.Space;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConsoleMultiplexer.Example
{
	class Player
	{
		public string Name { get; }
		public string Surname { get; }
		public DateTime BirthDate { get; }
		public int Points { get; }

		public Player(string name, string surname, DateTime birthDate, int points)
		{
			Name = name;
			Surname = surname;
			BirthDate = birthDate;
			Points = points;
		}
	}

	class InputController : IInputListener
	{
		private readonly TextBox _textBox;
		private readonly VerticalStackPanel _stackPanel;

		public InputController(TextBox textBox, VerticalStackPanel stackPanel)
		{
			_textBox = textBox;
			_stackPanel = stackPanel;
		}

		public void OnInput(InputEvent inputEvent)
		{
			if (inputEvent.Key.Key != ConsoleKey.Enter) return;

			_stackPanel.Add(new WrapPanel
			{
				Children = new IControl[]
				{
					new TextBlock {Text = $"[{DateTime.Now.ToLongTimeString()}] ", Color = new Color(200, 20, 20)},
					new TextBlock {Text = _textBox.Text}
				}
			});

			_textBox.Text = string.Empty;
			inputEvent.Handled = true;
		}
	}

	class Program
	{
		private static IControl BoardCell(char content, Color color)
		{
			return new Background
			{
				Color = color,
				Content = new Box
				{
					HorizontalContentPlacement = Box.HorizontalPlacement.Center,
					VerticalContentPlacement = Box.VerticalPlacement.Center,
					Content = new TextBlock { Text = content.ToString() }
				}
			};
		}

		static void Main()
		{
			var clock = new TextBlock();

			var canvas = new Canvas();
			var textBox = new TextBox();
			var consoleLog = new VerticalStackPanel();

			var board = new Grid
			{
				Rows = Enumerable.Repeat(new Grid.RowDefinition(3), 10).ToArray(),
				Columns = Enumerable.Repeat(new Grid.ColumnDefinition(5), 10).ToArray()
			};

			for (int i = 1; i < 9; i++)
			{
				var character = (char)('a' + (i - 1));
				var number = (char)('0' + (i - 1));
				var darkColor = new Color(50, 50, 50).Mix(Color.White, i % 2 == 1 ? 0f : 0.1f);
				var lightColor = new Color(50, 50, 50).Mix(Color.White, i % 2 == 0 ? 0f : 0.1f);

				board.AddChild(i, 0, BoardCell(character, darkColor));
				board.AddChild(i, 9, BoardCell(character, lightColor));
				board.AddChild(0, i, BoardCell(number, darkColor));
				board.AddChild(9, i, BoardCell(number, lightColor));
			}

			string[] pieces = new[] {
				"♜♞♝♛♚♝♞♜",
				"♟♟♟♟♟♟♟♟",
				"        ",
				"        ",
				"        ",
				"        ",
				"♙♙♙♙♙♙♙♙",
				"♖♘♗♕♔♗♘♖"
			};

			for (int i = 1; i < 9; i++)
				for (int j = 1; j < 9; j++)
					board.AddChild(i, j, BoardCell(pieces[j - 1][i - 1], new Color(139, 69, 19).Mix(Color.White, ((i + j) % 2) == 1 ? 0f : 0.4f)));

			var tabPanel = new TabPanel();
			tabPanel.AddTab("game", new Box
			{
				HorizontalContentPlacement = Box.HorizontalPlacement.Center,
				VerticalContentPlacement = Box.VerticalPlacement.Center,
				Content = board
			});

			tabPanel.AddTab("leaderboard", new Box
			{
				HorizontalContentPlacement = Box.HorizontalPlacement.Center,
				VerticalContentPlacement = Box.VerticalPlacement.Center,
				Content = new Background
				{
					Color = new Color(45, 74, 85),
					Content = new Border
					{
						BorderStyle = BorderStyle.Single,
						Content = new DataGrid<Player>
						{
							Columns = new[]
							{
								new DataGrid<Player>.ColumnDefinition("Name", 10, p => p.Name, foreground: p => p.Name == "Tomasz" ? (Color?)new Color(100, 100, 220) : null),
								new DataGrid<Player>.ColumnDefinition("Surname", 10, p => p.Surname),
								new DataGrid<Player>.ColumnDefinition("Birth date", 15, p => p.BirthDate.ToShortDateString()),
								new DataGrid<Player>.ColumnDefinition("Points", 5, p => p.Points.ToString(), background: p => p.Points > 20 ? (Color?)new Color(0, 220, 0) : null)
							},
							Data = new[]
							{
								new Player("John", "Connor", new DateTime(1985, 2, 28), 10),
								new Player("Ellen", "Ripley", new DateTime(2092, 1, 1), 23),
								new Player("Jan", "Kowalski", new DateTime(1990, 4, 10), 50),
								new Player("Tomasz", "Rewak", new DateTime(1900, 1, 1), 0),
							}
						}
					}
				}
			});

			tabPanel.AddTab("about", new Box
			{
				HorizontalContentPlacement = Box.HorizontalPlacement.Center,
				VerticalContentPlacement = Box.VerticalPlacement.Center,
				Content = new Boundary
				{
					MaxWidth = 20,
					Content = new VerticalStackPanel
					{
						Children = new IControl[]
						{
							new WrapPanel
							{
								Children = new[]
								{
									new TextBlock { Text = "This is just a demo application that uses a library for creating a GUI in a console." }
								}
							},
							new HorizontalSeparator(),
							new TextBlock {Text ="By Tomasz Rewak.", Color = new Color(200, 200, 200)}
						}
					}
				}
			});

			var dockPanel = new DockPanel
			{
				Placement = DockPanel.DockedContorlPlacement.Top,
				DockedControl = new DockPanel
				{
					Placement = DockPanel.DockedContorlPlacement.Right,
					DockedControl = new Background
					{
						Color = new Color(100, 100, 100),
						Content = new Boundary
						{
							MinWidth = 20,
							Content = new Box
							{
								Content = clock,
								HorizontalContentPlacement = Box.HorizontalPlacement.Center
							}
						}
					},
					FillingControl = new Background
					{
						Color = new Color(100, 0, 0),
						Content = new Box
						{
							Content = new TextBlock { Text = "Center" },
							HorizontalContentPlacement = Box.HorizontalPlacement.Center
						}
					}
				},
				FillingControl = new DockPanel
				{
					Placement = DockPanel.DockedContorlPlacement.Botton,
					DockedControl = new Boundary
					{
						MinHeight = 1,
						MaxHeight = 1,
						Content = new Background
						{
							Color = new Color(0, 100, 0),
							Content = new HorizontalStackPanel
							{
								Children = new IControl[] {
									new TextBlock { Text = " 10 ↑ " },
									new VerticalSeparator(),
									new TextBlock { Text = " 5 ↓ " }
								}
							}
						}
					},
					FillingControl = new Overlay
					{
						BottomContent = new Background
						{
							Color = new Color(25, 54, 65),
							Content = new DockPanel
							{
								Placement = DockPanel.DockedContorlPlacement.Right,
								DockedControl = new Background
								{
									Color = new Color(30, 40, 50),
									Content = new Border
									{
										BorderPlacement = BorderPlacement.Left,
										BorderStyle = BorderStyle.Double.WithColor(new Color(50, 60, 70)),
										Content = new Boundary
										{
											MinWidth = 50,
											MaxWidth = 50,
											Content = new DockPanel
											{
												Placement = DockPanel.DockedContorlPlacement.Botton,
												DockedControl = new WrapPanel
												{
													Children = new IControl[]
															{
																new Style
																{
																	Foreground = new Color(150, 150, 200),
																	Content = new TextBlock { Text = @"D:\Software\> " }
																},
																textBox
															}
												},
												FillingControl = new Box
												{
													VerticalContentPlacement = Box.VerticalPlacement.Bottom,
													HorizontalContentPlacement = Box.HorizontalPlacement.Stretch,
													Content = consoleLog
												}
											}
										}
									}
								},
								FillingControl = new DockPanel
								{
									Placement = DockPanel.DockedContorlPlacement.Right,
									DockedControl = new Background
									{
										Color = new Color(20, 30, 40),
										Content = new Border
										{
											BorderPlacement = BorderPlacement.Left,
											BorderStyle = BorderStyle.Double.WithColor(new Color(50, 60, 70)),
											Content = new Boundary
											{
												MinWidth = 30,
												MaxWidth = 30,
												Content = new Box
												{
													VerticalContentPlacement = Box.VerticalPlacement.Bottom,
													HorizontalContentPlacement = Box.HorizontalPlacement.Stretch,
													Content = new VerticalStackPanel
													{
														Children = new IControl[]
													{
														new WrapPanel
														{
															Children = new IControl[]
															{
																new Style
																{
																	Foreground = new Color(200, 20, 20),
																	Content = new TextBlock { Text = "[20:12:43] " }
																},
																new TextBlock { Text = "Some log line with a date to the left" }
															}
														},
														new WrapPanel
														{
															Children = new IControl[]
															{
																new Style
																{
																	Foreground = new Color(200, 20, 20),
																	Content = new TextBlock { Text = "[20:12:43] " }
																},
																new TextBlock { Text = "Some log line with a date to the left, but this time a little bit longer so that it wraps" }
															}
														},
													}
													}
												}
											}
										}
									},
									FillingControl = tabPanel
								}
							}
						},
						TopContent = canvas
					}
				}
			};

			var scrollPanel = new VerticalScrollPanel
			{
				Content = new VerticalStackPanel
				{
					Children = new IControl[]
					{
						new WrapPanel { Children = new [] { new TextBlock{Text = "Here is a short example of text wrapping" } } },
						new HorizontalSeparator(),
						new WrapPanel { Children = new [] { new TextBlock{Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum." } } },
					}
				}
			};

			canvas.Add(new Background
			{
				Color = new Color(10, 10, 10),
				Content = new VerticalStackPanel
				{
					Children = new IControl[]
					{
						new Border
						{
							BorderPlacement = BorderPlacement.Left | BorderPlacement.Top | BorderPlacement.Right,
							BorderStyle = BorderStyle.Double.WithColor(new Color(80, 80, 120)),
							Content = new Box
							{
								HorizontalContentPlacement = Box.HorizontalPlacement.Center,
								Content = new TextBlock { Text = "Popup 1", Color = new Color(200, 200, 100) }
							}
						},
						new Border
						{
							BorderPlacement = BorderPlacement.All,
							BorderStyle = BorderStyle.Double.WithTopLeft(new Character('╠')).WithTopRight(new Character('╣')).WithColor(new Color(80, 80, 120)),
							Content = scrollPanel
						}
					}
				}
			}, new Rect(77, 5, 30, 10));

			canvas.Add(new Background
			{
				Color = new Color(10, 40, 10),
				Content = new Border
				{
					Content = new Box
					{
						HorizontalContentPlacement = Box.HorizontalPlacement.Center,
						VerticalContentPlacement = Box.VerticalPlacement.Center,
						Content = new TextBlock { Text = "Popup 2" }
					}
				}
			}, new Rect(66, 12, 17, 5));

			ConsoleManager.Setup();
			ConsoleManager.Resize(new Size(150, 40));
			ConsoleManager.Content = dockPanel;

			var input = new IInputListener[]
			{
				scrollPanel,
				tabPanel,
				new InputController(textBox, consoleLog),
				textBox
			};

			while (true)
			{
				Thread.Sleep(20);

				clock.Text = DateTime.Now.ToLongTimeString();

				ConsoleManager.ReadInput(input);
			}
		}
	}
}
