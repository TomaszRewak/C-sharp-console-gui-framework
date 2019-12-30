using ConsoleGUI.Data;
using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Api
{
	public interface IConsole
	{
		Size Size { get; set; }
		bool KeyAvailable { get; }

		void Initialize();
		void OnRefresh();
		void Write(Position position, in Character character);
		ConsoleKeyInfo ReadKey();
	}
}
