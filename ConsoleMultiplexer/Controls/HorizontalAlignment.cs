using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class HorizontalAlignment : Control
	{
		private DrawingContext _contentContext;
		private DrawingContext ContentContext
		{
			get => _contentContext;
			set => Setter
				.SetDisposable(ref _contentContext, value)
				.Then(Resize);
		}

		private IControl _content;
		public IControl Content
		{
			get => _content;
			set => Setter
				.Set(ref _content, value)
				.Then(BindContent);
		}

		private int ContentOffset => (Size.Width - Content?.Size.Width ?? 0) / 2;

		public override Character this[Position position]
		{
			get
			{
				var contentPosition = position.Move(-ContentOffset, 0);

				if (Content.Size.Contains(contentPosition))
					return Content[contentPosition];

				return Character.Empty;
			}
		}

		protected override void Resize()
		{
			using (Freeze())
			{
				ContentContext?.SetLimits(
					new Size(0, MinSize.Height),
					MaxSize);

				var newSize = Size.Clip(MinSize, Content?.Size ?? Size.Empty, MaxSize);

				ContentContext?.SetOffset(new Vector((Size.Width - Content?.Size.Width ?? 0) / 2, 0));

				Redraw(newSize);
			}
		}

		private void BindContent()
		{
			ContentContext = new DrawingContext(Content, Resize, OnContentUpdateRequested);
		}

		private void OnContentUpdateRequested(Rect rect)
		{
			Update(rect);
		}
	}
}
