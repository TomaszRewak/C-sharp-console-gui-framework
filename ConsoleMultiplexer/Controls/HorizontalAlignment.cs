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
				.SetDisposable(ref _contentContext, value);
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

				Redraw(MaxSize);
			}
		}

		private void BindContent()
		{
			ContentContext = new DrawingContext(Content, OnContentRedrawRequested, OnContentUpdateRequested);
		}

		private void OnContentRedrawRequested()
		{
			Redraw(Size.Clip(MinSize, Content?.Size ?? Size.Empty, MaxSize));
		}

		private void OnContentUpdateRequested(Rect rect)
		{
			Update(rect.Move(new Vector(ContentOffset, 0)));
		}
	}
}
