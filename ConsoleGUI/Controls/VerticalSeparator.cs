using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Controls
{
	public sealed class VerticalSeparator : Control
	{
		private Character character = new Character('│');
		public Character Character
		{
			get => character;
			set => Setter
				.Set(ref character, value)
				.Then(Initialize);
		}

		public override Cell this[Position position] => Character;

		protected override void Initialize()
		{
			Resize(new Size(1, MinSize.Height));
		}
	}
}
