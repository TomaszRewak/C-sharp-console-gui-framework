using ConsoleGUI.Buffering;
using ConsoleGUI.Common;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleGUI
{
	public static class ConsoleManager
	{
		private class ConsoleManagerDrawingContextListener : IDrawingContextListener
		{
			void IDrawingContextListener.OnRedraw(DrawingContext drawingContext)
			{
				if (_freezeLock.IsFrozen) return;
				Redraw();
			}

			void IDrawingContextListener.OnUpdate(DrawingContext drawingContext, Rect rect)
			{
				if (_freezeLock.IsFrozen) return;
				Update(rect);
			}
		}

		private static readonly ConsoleBuffer _buffer = new ConsoleBuffer();
		private static FreezeLock _freezeLock;

		private static DrawingContext _contentContext = DrawingContext.Dummy;
		private static DrawingContext ContentContext
		{
			get => _contentContext;
			set => Setter
				.SetDisposable(ref _contentContext, value)
				.Then(Initialize);
		}

		private static IControl _content;
		public static IControl Content
		{
			get => _content;
			set => Setter
				.Set(ref _content, value)
				.Then(BindContent);
		}

		private static bool _compatibilityMode;
		public static bool CompatibilityMode
		{
			get => _compatibilityMode;
			set => Setter
				.Set(ref _compatibilityMode, value)
				.Then(Redraw);
		}

		private static bool _dontPrintTheLastCharacter;
		public static bool DontPrintTheLastCharacter
		{
			get => _dontPrintTheLastCharacter;
			set => Setter
				.Set(ref _dontPrintTheLastCharacter, value)
				.Then(Redraw);
		}

		private static Position? _mousePosition;
		private static Position? MousePosition
		{
			get => _mousePosition;
			set => Setter
				.Set(ref _mousePosition, value)
				.Then(UpdateMouseContext);
		}

		private static MouseContext? _mouseContext;
		private static MouseContext? MouseContext
		{
			get => _mouseContext;
			set
			{
				if (value?.MouseListener != _mouseContext?.MouseListener)
				{
					_mouseContext?.MouseListener.OnMouseLeave();
					value?.MouseListener.OnMouseEnter();
					value?.MouseListener.OnMouseMove(value.Value.RelativePosition);
				}
				else if (value.HasValue && value.Value.RelativePosition != _mouseContext?.RelativePosition)
				{
					value.Value.MouseListener.OnMouseMove(value.Value.RelativePosition);
				}

				_mouseContext = value;
			}
		}

		public static Size WindowSize => new Size(Console.WindowWidth, Console.WindowHeight);
		public static Size BufferSize => _buffer.Size;

		private static void Initialize()
		{
			var consoleSize = BufferSize;

			Console.Clear();

			_freezeLock.Freeze();
			ContentContext.SetLimits(consoleSize, consoleSize);
			_freezeLock.Unfreeze();

			Redraw();
		}

		private static void Redraw()
		{
			Update(ContentContext.Size.AsRect());
		}

		private static void Update(Rect rect)
		{
			SafeConsole.HideCursor();

			var lastPoint = WindowSize.AsRect().BottomRightCorner;
			rect = Rect.Intersect(rect, Rect.OfSize(BufferSize));
			rect = Rect.Intersect(rect, Rect.OfSize(WindowSize));

			for (int y = rect.Top; y <= rect.Bottom; y++)
			{
				for (int x = rect.Left; x <= rect.Right; x++)
				{
					var position = new Position(x, y);

					if (DontPrintTheLastCharacter && position == lastPoint)
						continue;

					var cell = ContentContext[position];

					if (!_buffer.Update(position, cell)) continue;

					var content = cell.Content ?? ' ';
					var foreground = cell.Foreground ?? Color.White;
					var background = cell.Background ?? Color.Black;

					if (content == '\n') content = ' ';

					try
					{
						Console.SetCursorPosition(position.X, position.Y);

						if (CompatibilityMode)
						{
							Console.BackgroundColor = ColorConverter.GetNearestConsoleColor(background);
							Console.ForegroundColor = ColorConverter.GetNearestConsoleColor(foreground);
							Console.Write(content);
						}
						else
						{
							Console.Write($"\x1b[38;2;{foreground.Red};{foreground.Green};{foreground.Blue}m\x1b[48;2;{background.Red};{background.Green};{background.Blue}m{content}");
						}
					}
					catch (ArgumentOutOfRangeException)
					{
						rect = Rect.Intersect(rect, Rect.OfSize(WindowSize));
					}
				}
			}
		}

		public static void Setup()
		{
			SafeConsole.SetUtf8();
			SafeConsole.HideCursor();
			Resize(WindowSize);
			Initialize();
		}

		public static void Resize(in Size size)
		{
			Console.SetCursorPosition(0, 0);
			SafeConsole.SetWindowPosition(0, 0);
			if (!(WindowSize <= size)) SafeConsole.SetWindowSize(1, 1);
			ResizeBuffer(size);
			if (WindowSize != size) SafeConsole.SetWindowSize(size.Width, size.Height);
			Initialize();
		}

		public static void AdjustBufferSize()
		{
			if (WindowSize != BufferSize)
				Resize(WindowSize);
		}

		public static void AdjustWindowSize()
		{
			if (WindowSize != BufferSize)
				Resize(BufferSize);
		}

		private static void ResizeBuffer(in Size size)
		{
			_buffer.Initialize(size);
			SafeConsole.SetBufferSize(size.Width, size.Height);
		}

		public static void ReadInput(IReadOnlyCollection<IInputListener> controls)
		{
			while (Console.KeyAvailable)
			{
				var key = Console.ReadKey(true);
				var inputEvent = new InputEvent(key);

				foreach (var control in controls)
				{
					control.OnInput(inputEvent);
					if (inputEvent.Handled) break;
				}
			}
		}

		public static void OnMouseMove(in Position position)
		{
			MousePosition = position;
		}

		public static void OnMouseUp(in Position position)
		{
			MousePosition = position;
			MouseContext?.MouseListener.OnMouseUp(MouseContext.Value.RelativePosition);
		}

		public static void OnMouseDonw(in Position position)
		{
			MousePosition = position;
			MouseContext?.MouseListener.OnMouseDown(MouseContext.Value.RelativePosition);
		}

		public static void OnMouseLeave()
		{
			MousePosition = null;
		}

		private static void BindContent()
		{
			ContentContext = new DrawingContext(new ConsoleManagerDrawingContextListener(), Content);
		}

		private static void UpdateMouseContext()
		{
			MouseContext = MousePosition.HasValue
				? _buffer.GetMouseContext(MousePosition.Value)
				: null;
		}
	}
}
