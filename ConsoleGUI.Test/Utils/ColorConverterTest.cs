using ConsoleGUI.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Test.Utils
{
	[TestFixture]
	class ColorConverterTest
	{
		[TestCase(ConsoleColor.Black)]
		[TestCase(ConsoleColor.DarkBlue)]
		[TestCase(ConsoleColor.DarkGreen)]
		[TestCase(ConsoleColor.DarkCyan)]
		[TestCase(ConsoleColor.DarkRed)]
		[TestCase(ConsoleColor.DarkMagenta)]
		[TestCase(ConsoleColor.DarkYellow)]
		[TestCase(ConsoleColor.Gray)]
		[TestCase(ConsoleColor.DarkGray)]
		[TestCase(ConsoleColor.Blue)]
		[TestCase(ConsoleColor.Green)]
		[TestCase(ConsoleColor.Cyan)]
		[TestCase(ConsoleColor.Red)]
		[TestCase(ConsoleColor.Magenta)]
		[TestCase(ConsoleColor.Yellow)]
		[TestCase(ConsoleColor.White)]
		public void ConsoleColor_IsRestoredCorrectly(ConsoleColor initialColor)
		{
			var color = ColorConverter.GetColor(initialColor);
			var convertedColor = ColorConverter.GetNearestConsoleColor(color);

			Assert.AreEqual(initialColor, convertedColor);
		}
	}
}
