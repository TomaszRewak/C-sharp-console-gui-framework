using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGUI.MouseExample
{
	public static class MouseHandler
	{
		private static IntPtr _inputHandle = IntPtr.Zero;
		private static INPUT_RECORD[] _inputBuffer;

		public static Position MousePosition { get; private set; }

		public static void Initialize()
		{
			_inputHandle = GetStdHandle(unchecked((uint)-10));
			_inputBuffer = new INPUT_RECORD[100];
		}

		public static void Read()
		{
			if (_inputHandle == IntPtr.Zero)
				throw new InvalidOperationException("First call the Initialize method of the MouseHandler");

			if (!ReadConsoleInput(_inputHandle, _inputBuffer, (uint)_inputBuffer.Length, out var eventsRead)) return;

			for (int i = 0; i < eventsRead; i++)
			{
				var inputEvent = _inputBuffer[i];

				if (inputEvent.EventType == INPUT_RECORD.MOUSE_EVENT)
					ProcessMouseEvent(inputEvent.MouseEvent);
				else
					WriteConsoleInput(_inputHandle, new[] { inputEvent }, 1, out var eventsWritten);
			}
		}

		private static void ProcessMouseEvent(in MOUSE_EVENT_RECORD mouseEvent)
		{
			MousePosition = new Position(mouseEvent.dwMousePosition.X, mouseEvent.dwMousePosition.Y);
		}

		private struct COORD
		{
			public short X;
			public short Y;
		}

		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		private struct KEY_EVENT_RECORD
		{
			[FieldOffset(0)]
			public bool bKeyDown;
			[FieldOffset(4)]
			public ushort wRepeatCount;
			[FieldOffset(6)]
			public ushort wVirtualKeyCode;
			[FieldOffset(8)]
			public ushort wVirtualScanCode;
			[FieldOffset(10)]
			public char UnicodeChar;
			[FieldOffset(10)]
			public byte AsciiChar;
			[FieldOffset(12)]
			public uint dwControlKeyState;
		}

		private struct MOUSE_EVENT_RECORD
		{
			public COORD dwMousePosition;

			public uint dwButtonState;
			public const uint
				FROM_LEFT_1ST_BUTTON_PRESSED = 0x0001,
				FROM_LEFT_2ND_BUTTON_PRESSED = 0x0004,
				FROM_LEFT_3RD_BUTTON_PRESSED = 0x0008,
				FROM_LEFT_4TH_BUTTON_PRESSED = 0x0010,
				RIGHTMOST_BUTTON_PRESSED = 0x0002;

			public uint dwControlKeyState;
			public const int
				CAPSLOCK_ON = 0x0080,
				ENHANCED_KEY = 0x0100,
				LEFT_ALT_PRESSED = 0x0002,
				LEFT_CTRL_PRESSED = 0x0008,
				NUMLOCK_ON = 0x0020,
				RIGHT_ALT_PRESSED = 0x0001,
				RIGHT_CTRL_PRESSED = 0x0004,
				SCROLLLOCK_ON = 0x0040,
				SHIFT_PRESSED = 0x0010;

			public uint dwEventFlags;
			public const int
				DOUBLE_CLICK = 0x0002,
				MOUSE_HWHEELED = 0x0008,
				MOUSE_MOVED = 0x0001,
				MOUSE_WHEELED = 0x0004;
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct INPUT_RECORD
		{
			public const ushort MOUSE_EVENT = 0x0002;

			[FieldOffset(0)]
			public ushort EventType;
			[FieldOffset(4)]
			public KEY_EVENT_RECORD KeyEvent;
			[FieldOffset(4)]
			public MOUSE_EVENT_RECORD MouseEvent;
		}

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetStdHandle(uint nStdHandle);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern bool ReadConsoleInput(IntPtr hConsoleInput, [Out] INPUT_RECORD[] lpBuffer, uint nLength, out uint lpNumberOfEventsRead);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern bool WriteConsoleInput(IntPtr hConsoleInput, INPUT_RECORD[] lpBuffer, uint nLength, out uint lpNumberOfEventsWritten);
	}
}
