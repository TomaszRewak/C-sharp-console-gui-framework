using System;
using System.Threading;

namespace ConsoleMultiplexer.Example
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.SetWindowSize(1, 1);
			Console.SetBufferSize(200, 50);
			Console.SetWindowSize(200, 50);

			var consoleHandle1 = new WindowHandle(new Rect(10, 10, 10, 10), WindowBorder.All);
			var consoleHandle2 = new WindowHandle(new Rect(20, 20, 10, 10), WindowBorder.All);
			var consoleHandle3 = new WindowHandle(new Rect(25, 15, 20, 10), WindowBorder.All);

			for (int i = 0; ; i++)
			{

				Thread.Sleep(500);
			}
		}
	}
}
