using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class TextBlock : Control
	{
		private string _text = "";
		public string Text
		{
			get => _text;
			set => Setter
				.Set(ref _text, value)
				.Then(Resize);
		}

		public override Character this[Position position]
		{
			get
			{
				if (Text == null) return Character.Empty;
				if (position.X >= Text.Length) return Character.Empty;
				if (position.Y >= 1) return Character.Empty;
				return Character.Plain(Text[position.X]);
			}
		}

		protected override void Resize()
		{
			var newSize = Size.Clip(MinSize, new Size(Text.Length, 1), MaxSize);

			Redraw(newSize);
		}
	}
}
