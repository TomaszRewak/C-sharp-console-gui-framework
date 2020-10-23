using System;
using System.Collections.Generic;
using System.Text;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;

namespace ConsoleGUI.Api
{
	public class SimplifiedConsole : StandardConsole
	{
		private Position _lastPosition;

		public override void Write(Position position, in Character character)
		{
			if (position == _lastPosition) return;

			var content = character.Content ?? ' ';
			var foreground = character.Foreground ?? Color.White;
			var background = character.Background ?? Color.Black;

			if (content == '\n') content = ' ';

			SafeConsole.WriteOrThrow(
				position.X,
				position.Y,
				ColorConverter.GetNearestConsoleColor(background),
				ColorConverter.GetNearestConsoleColor(foreground),
				content);
		}

		public override void OnRefresh()
		{
			base.OnRefresh();
			_lastPosition = Size.AsRect().BottomRightCorner;
		}
	}
}
