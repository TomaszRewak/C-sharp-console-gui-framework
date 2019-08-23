using ConsoleMultiplexer.DataStructures;
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
	}

	internal class BorderContext : IDrawingContext
	{
		private CharacterBuffer _contentBuffer = new CharacterBuffer();
		private bool _requiresRepainting = true;
		private Size _previousContentSize;

		public WindowBorder Border => WindowBorder.All;

		public IDrawingContext BaseContext { get; }

		public int? Width { get; }
		public int? Height { get; }

		public int? ContentWidth => Width - 2;
		public int? ContentHeight => Height - 2;

		public Size MinSize => Size.Of(ContentWidth ?? 0, ContentHeight ?? 0);
		public Size MaxSize => Size.Of(ContentWidth ?? int.MaxValue, ContentHeight ?? int.MaxValue);

		public BorderContext(IDrawingContext baseContext, int? width, int? height)
		{
			BaseContext = baseContext;

			Width = width;
			Height = height;
		}

		public void Clear()
		{
			_contentBuffer.Clear();
		}

		public void Flush()
		{
			CheckIfRequiresRepainting();

			if (_requiresRepainting)
				Repaint();
			else
				FlushChanges();
		}

		public void Set(in Position position, in Character character)
		{
			_contentBuffer.Set(position, character);
		}

		private void Repaint()
		{
			_requiresRepainting = false;
			_contentBuffer.ClearChanges();

			var size = Size.Intersection(MaxSize, _contentBuffer.Size).Expand(2, 2);

			if (Border.HasFlag(WindowBorder.Top))
				for (int i = 1; i < size.Width - 1; i++)
					BaseContext.Set(Position.At(i, 0), Character.Plain('═'));

			if (Border.HasFlag(WindowBorder.Bottom))
				for (int i = 1; i < size.Width - 1; i++)
					BaseContext.Set(Position.At(i, size.Height - 1), Character.Plain('═'));

			if (Border.HasFlag(WindowBorder.Left))
				for (int i = 1; i < size.Height - 1; i++)
					BaseContext.Set(Position.At(0, i), Character.Plain('║'));

			if (Border.HasFlag(WindowBorder.Right))
				for (int i = 1; i < size.Height - 1; i++)
					BaseContext.Set(Position.At(size.Width - 1, i), Character.Plain('║'));

			if (Border.HasFlag(WindowBorder.Top | WindowBorder.Left))
				BaseContext.Set(Position.At(0, 0), Character.Plain('╔'));

			if (Border.HasFlag(WindowBorder.Top | WindowBorder.Right))
				BaseContext.Set(Position.At(size.Width - 1, 0), Character.Plain('╗'));

			if (Border.HasFlag(WindowBorder.Bottom | WindowBorder.Left))
				BaseContext.Set(Position.At(0, size.Height - 1), Character.Plain('╚'));

			if (Border.HasFlag(WindowBorder.Bottom | WindowBorder.Right))
				BaseContext.Set(Position.At(size.Width - 1, size.Height - 1), Character.Plain('╝'));

			foreach (var position in _contentBuffer.Size)
				BaseContext.Set(position.Move(1, 1), _contentBuffer.Get(position));

			BaseContext.Flush();
		}

		private void FlushChanges()
		{
			foreach (var change in _contentBuffer.Changes)
				BaseContext.Set(change.Move(1, 1), _contentBuffer.Get(change));
			_contentBuffer.ClearChanges();

			BaseContext.Flush();
		}

		private void CheckIfRequiresRepainting()
		{
			if (_contentBuffer.Size == _previousContentSize) return;
			_previousContentSize = _contentBuffer.Size;
			_requiresRepainting = true;
		}
	}
}
