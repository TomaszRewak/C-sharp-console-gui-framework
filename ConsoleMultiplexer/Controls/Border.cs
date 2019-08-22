using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class Border : IControl
	{
		public IDrawingContext Context { get; set; }

		private int? _width;
		public int? Width
		{
			get => _width;
			set 
			{
				if (_width == value) return;
				_width = value;
				Draw();
			}
		}

		private int? _height;
		public int? Height
		{
			get => _height;
			set
			{
				if (_height == value) return;
				_height = value;
				Draw();
			}
		}

		public void Draw()
		{
			if (Context == null) return;

			Context.Clear();
			Context.Flush();
		}

		private void DrawBox()
		{

		}

		private void DrawContent()
		{

		}
	}

	internal class BorderContext : IDrawingContext
	{
		public IDrawingContext BaseContext { get; }

		public int? Width { get; }
		public int? Height { get; }

		public int? ContentWidth => Width - 2;
		public int? ContentHeight => Height - 2;

		public Size MinSize => Size.Of(ContentWidth ?? 0, ContentHeight ?? 0);
		public Size MaxSize => Size.Of(ContentWidth ?? int.MaxValue, ContentHeight ?? int.MaxValue);

		private Size ActualContentSize 

		public BorderContext(IDrawingContext baseContext, int? width, int? height)
		{
			BaseContext = baseContext;

			Width = width;
			Height = height;
		}

		public void Clear()
		{
			BaseContext.Clear();
		}

		public void Flush()
		{
			throw new NotImplementedException();
		}

		public void Set(Position position, Character character)
		{
			throw new NotImplementedException();
		}
	}
}
