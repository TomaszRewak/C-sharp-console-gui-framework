using ConsoleMultiplexer.Common;
using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Space;
using ConsoleMultiplexer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
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

		public override Character this[Position position] => Character;

		protected override void Initialize()
		{
			Resize(new Size(1, MinSize.Height));
		}
	}
}
