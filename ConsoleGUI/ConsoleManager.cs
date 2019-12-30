using ConsoleGUI.Api;
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

		private static IConsole _console = new StandardConsole();
		public static IConsole Console
		{
			get => _console;
			set => Setter
				.Set(ref _console, value)
				.Then(Initialize);
		}

		private static Position? _mousePosition;
		public static Position? MousePosition
		{
			get => _mousePosition;
			set => Setter
				.Set(ref _mousePosition, value)
				.Then(UpdateMouseContext);
		}

		private static bool _mouseDown;
		public static bool MouseDown
		{
			get => _mouseDown;
			set
			{
				if (_mouseDown && !value)
					MouseContext?.MouseListener?.OnMouseUp(MouseContext.Value.RelativePosition);
				if (!_mouseDown && value)
					MouseContext?.MouseListener?.OnMouseDown(MouseContext.Value.RelativePosition);

				_mouseDown = value;
			}
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

		public static Size WindowSize => Console.Size;
		public static Size BufferSize => _buffer.Size;

		private static void Initialize()
		{
			var consoleSize = BufferSize;

			Console.Initialize();

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
			Console.OnRefresh();

			rect = Rect.Intersect(rect, Rect.OfSize(BufferSize));
			rect = Rect.Intersect(rect, Rect.OfSize(WindowSize));

			for (int y = rect.Top; y <= rect.Bottom; y++)
			{
				for (int x = rect.Left; x <= rect.Right; x++)
				{
					var position = new Position(x, y);

					var cell = ContentContext[position];

					if (!_buffer.Update(position, cell)) continue;

					try
					{
						Console.Write(position, cell.Character);
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
			Resize(WindowSize);
		}

		public static void Resize(in Size size)
		{
			Console.Size = size;
			_buffer.Initialize(size);

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

		public static void ReadInput(IReadOnlyCollection<IInputListener> controls)
		{
			while (Console.KeyAvailable)
			{
				var key = Console.ReadKey();
				var inputEvent = new InputEvent(key);

				foreach (var control in controls)
				{
					control?.OnInput(inputEvent);
					if (inputEvent.Handled) break;
				}
			}
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
