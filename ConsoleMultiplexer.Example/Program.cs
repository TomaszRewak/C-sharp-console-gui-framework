using System;
using System.Threading;

namespace ConsoleMultiplexer.Example
{
	class Program
	{


		static void Main(string[] args)
		{
			Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

			var consoleHandle1 = new ConsoleWindowHandle(10, 10, 10, 10);
			var consoleHandle2 = new ConsoleWindowHandle(20, 20, 10, 10);

			for (int i = 0; ; i++)
			{
				consoleHandle1.WriteLine($"aaa{i}");
				consoleHandle1.WriteLine($"bbb{i}");
				consoleHandle1.WriteLine($"ccc{i}");
				consoleHandle2.WriteLine($"ddd{i}");
				consoleHandle2.WriteLine($"eee{i}");

				Thread.Sleep(500);
			}
		}
	}
}
