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

		public static void Initialize()
		{
			_inputHandle = GetStdHandle(unchecked((uint)-10));
			_inputBuffer = new INPUT_RECORD[100];
		}

		public static void ReadMouseEvents()
		{
			if (_inputHandle == IntPtr.Zero)
				throw new InvalidOperationException("First call the Initialize method of the MouseHandler");

			if (!ReadConsoleInput(_inputHandle, _inputBuffer, (uint)_inputBuffer.Length, out var eventsRead)) return;

			for (int i = 0; i < eventsRead; i++)
			{
				var inputEvent = _inputBuffer[i];

				if ((inputEvent.EventType & 0x0002) != 0)
					ProcessMouseEvent(inputEvent.MouseEvent);
				else
					WriteConsoleInput(_inputHandle, new[] { inputEvent }, 1, out var eventsWritten);
			}
		}

		private static void ProcessMouseEvent(in MOUSE_EVENT_RECORD mouseEvent)
		{
			ConsoleManager.MousePosition = new Position(mouseEvent.dwMousePosition.X, mouseEvent.dwMousePosition.Y);
			ConsoleManager.MouseDown = (mouseEvent.dwButtonState & 0x0001) != 0;
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
			public uint dwControlKeyState;
			public uint dwEventFlags;
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct INPUT_RECORD
		{
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
