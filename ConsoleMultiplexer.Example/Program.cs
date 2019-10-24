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
		static void Main()
		{
			var clock = new TextBlock();

			var canvas = new Canvas();
			var textBox = new TextBox();
			var consoleLog = new VerticalStackPanel();

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
																	Content = new TextBlock {Text = @"D:\Software\> "}
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
																	Content = new TextBlock {Text = "[20:12:43] "}
																},
																new TextBlock {Text = "Some log line with a date to the left"}
															}
														},
														new WrapPanel
														{
															Children = new IControl[]
															{
																new Style
																{
																	Foreground = new Color(200, 20, 20),
																	Content = new TextBlock {Text = "[20:12:43] "}
																},
																new TextBlock {Text = "Some log line with a date to the left, but this time a little bit longer so that it wraps"}
															}
														},
													}
													}
												}
											}
										}
									},
									FillingControl = new Background { Color = new Color(255, 0, 0) }
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
