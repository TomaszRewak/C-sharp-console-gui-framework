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
		private Size _contentSize;


		private IDrawingContext _context;
		public IDrawingContext Context
		{
			get => _context;
			set => Setter
				.Set(ref _context, value)
				.OnSizeChanged(c => Draw())
				.Then(Draw);
		}

		private IControl _content;
		public IControl Content
		{
			get => _content;
			set => Setter
				.Set(ref _content, value)
				.Then(RepaintContent);
		}

		public void Draw()
		{
			if (Context == null) return;

			Context.Clear();
			Context.Flush();
		}
			   
		Size IDrawingContext.MinSize => Context.MinSize.Shrink(2, 2);
		Size IDrawingContext.MaxSize => Context.MaxSize.Shrink(2, 2);

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
	}
}
