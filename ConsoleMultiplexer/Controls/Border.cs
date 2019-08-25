using ConsoleMultiplexer.DataStructures;
using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class Border : IControl, IDrawingContext
	{
		private CharacterBuffer _contentBuffer = new CharacterBuffer();
		private UIContext _context = UIContext.Empty;

		private IControl _content;
		public IControl Content
		{
			get => _content;
			set => Setter
				.Set(ref _content, value)
				.Then(Draw);
		}

		private BorderPlacement _borderPlacement;
		public BorderPlacement BorderPlacement
		{
			get => _borderPlacement;
			set => Setter
				.Set(ref _borderPlacement, value)
				.Then(Draw);
		}

		public Size Size => Size.Intersection(_contentBuffer.Size.Expand(2, 2), _context.MinSize);

		public void Draw(UIContext context)
		{
			_context = context;
			Draw();
		}

		private void Draw()
		{
			Content.Draw(new UIContext(this, _context.MinSize.Shrink(2, 2), _context.MaxSize.Shrink(2, 2)));
		}

		private void DrawBorder()
		{
			if (BorderPlacement.HasFlag(BorderPlacement.Top))
				for (int i = 1; i < Size.Width - 1; i++)
					_context.Set(Position.At(i, 0), Character.Plain('═'));

			if (BorderPlacement.HasFlag(BorderPlacement.Bottom))
				for (int i = 1; i < Size.Width - 1; i++)
					_context.Set(Position.At(i, Size.Height - 1), Character.Plain('═'));

			if (BorderPlacement.HasFlag(BorderPlacement.Left))
				for (int i = 1; i < Size.Height - 1; i++)
					_context.Set(Position.At(0, i), Character.Plain('║'));

			if (BorderPlacement.HasFlag(BorderPlacement.Right))
				for (int i = 1; i < Size.Height - 1; i++)
					_context.Set(Position.At(Size.Width - 1, i), Character.Plain('║'));

			if (BorderPlacement.HasFlag(BorderPlacement.Top | BorderPlacement.Left))
				_context.Set(Position.At(0, 0), Character.Plain('╔'));

			if (BorderPlacement.HasFlag(BorderPlacement.Top | BorderPlacement.Right))
				_context.Set(Position.At(Size.Width - 1, 0), Character.Plain('╗'));

			if (BorderPlacement.HasFlag(BorderPlacement.Bottom | BorderPlacement.Left))
				_context.Set(Position.At(0, Size.Height - 1), Character.Plain('╚'));

			if (BorderPlacement.HasFlag(BorderPlacement.Bottom | BorderPlacement.Right))
				_context.Set(Position.At(Size.Width - 1, Size.Height - 1), Character.Plain('╝'));
		}

		private void DrawBackground()
		{

		}

		void IDrawingContext.Clear()
		{
			_contentBuffer.Clear();
		}

		void IDrawingContext.Flush()
		{
			if (_contentBuffer.Size == _contentSize)
				Repaint();
			else
				FlushChanges();
		}

		void IDrawingContext.Set(in Position position, in Character character)
		{
			_contentBuffer.Set(position, character);
		}

		private void FlushChanges()
		{
			foreach (var change in _contentBuffer.Changes)
				BaseContext.Set(change.Move(1, 1), _contentBuffer.Get(change));
			_contentBuffer.ClearChanges();

			BaseContext.Flush();
		}
	}
}
