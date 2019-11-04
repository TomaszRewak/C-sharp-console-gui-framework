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
		private static InputRecord[] _inputBuffer;

		public static void Initialize()
		{
			_inputHandle = GetStdHandle(unchecked((uint)-10));
			_inputBuffer = new InputRecord[100];
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

		private static void ProcessMouseEvent(in MouseRecord mouseEvent)
		{
			ConsoleManager.MousePosition = new Position(mouseEvent.MousePosition.X, mouseEvent.MousePosition.Y);
			ConsoleManager.MouseDown = (mouseEvent.ButtonState & 0x0001) != 0;
		}

		private struct COORD
		{
			public short X;
			public short Y;
		}

		private struct MouseRecord
		{
			public COORD MousePosition;
			public uint ButtonState;
			public uint ControlKeyState;
			public uint EventFlags;
		}

		private struct InputRecord
		{
			public ushort EventType;
			public MouseRecord MouseEvent;
		}

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetStdHandle(uint nStdHandle);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern bool ReadConsoleInput(IntPtr hConsoleInput, [Out] InputRecord[] lpBuffer, uint nLength, out uint lpNumberOfEventsRead);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern bool WriteConsoleInput(IntPtr hConsoleInput, InputRecord[] lpBuffer, uint nLength, out uint lpNumberOfEventsWritten);
	}
}
