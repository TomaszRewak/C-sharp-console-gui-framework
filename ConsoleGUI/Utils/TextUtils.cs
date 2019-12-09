using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Utils
{
	internal static class TextUtils
	{
		public static int PreviousLine(string text, int position)
		{
			var frontOfThisLine = Front(text, position);
			if (frontOfThisLine == 0) return 0;

			var previousLine = Front(text, frontOfThisLine - 1);
			var leftOffset = LeftOffset(text, position);
			if (previousLine + leftOffset >= frontOfThisLine) return frontOfThisLine - 1;

			return previousLine + leftOffset;
		}

		public static int NextLine(string text, int position)
		{
			var backOfThisLine = Back(text, position);
			if (backOfThisLine == text.Length - 1) return backOfThisLine;

			var newLine = backOfThisLine + 1;
			var leftOffset = LeftOffset(text, position);
			var backOfNextLine = Back(text, newLine);
			if (newLine + leftOffset > backOfNextLine) return backOfNextLine;

			return newLine + leftOffset;
		}

		private static int LeftOffset(string text, int position)
		{
			return position - Front(text, position);
		}

		private static int Front(string text, int position)
		{
			while (position > 0 && text[position - 1] != '\n') position--;
			return position;
		}

		private static int Back(string text, int position)
		{
			while (position < text.Length - 1 && text[position] != '\n') position++;
			return position;
		}
	}
}
