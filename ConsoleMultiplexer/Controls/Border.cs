using ConsoleMultiplexer.DataStructures;
using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public sealed class Border : Control
	{
		private readonly BorderContext _contentContext;

		public Border()
		{
			_contentContext = new BorderContext(this);
		}

		private IControl _content;
		public IControl Content
		{
			get => _content;
			set => Setter
				.Set(ref _content, value)
				.ThenSetContext(_contentContext);
		}

		private BorderPlacement _borderPlacement = BorderPlacement.All;
		public BorderPlacement BorderPlacement
		{
			get => _borderPlacement;
			set => Setter
				.Set(ref _borderPlacement, value)
				.Then(Resize);
		}

		public override Character this[Position position]
		{
			get
			{
				if (!Size.Contains(position)) throw new IndexOutOfRangeException(nameof(position));

				if (position.X == 0 && position.Y == 0 && BorderPlacement.HasFlag(BorderPlacement.Top | BorderPlacement.Left))
					return Character.Plain('╔');

				if (position.X == Size.Width - 1 && position.Y == 0 && BorderPlacement.HasFlag(BorderPlacement.Top | BorderPlacement.Right))
					return Character.Plain('╗');

				if (position.X == 0 && position.Y == Size.Height - 1 && BorderPlacement.HasFlag(BorderPlacement.Bottom | BorderPlacement.Left))
					return Character.Plain('╚');

				if (position.X == Size.Width - 1 && position.Y == Size.Height - 1 && BorderPlacement.HasFlag(BorderPlacement.Bottom | BorderPlacement.Right))
					return Character.Plain('╝');

				if (position.X == 0 && BorderPlacement.HasFlag(BorderPlacement.Left))
					return Character.Plain('║');

				if (position.X == Size.Width - 1 && BorderPlacement.HasFlag(BorderPlacement.Right))
					return Character.Plain('║');

				if (position.Y == 0 && BorderPlacement.HasFlag(BorderPlacement.Top))
					return Character.Plain('═');

				if (position.Y == Size.Height - 1 && BorderPlacement.HasFlag(BorderPlacement.Bottom))
					return Character.Plain('═');

				var contentPosition = position.Move(
					BorderPlacement.HasFlag(BorderPlacement.Left) ? -1 : 0,
					BorderPlacement.HasFlag(BorderPlacement.Top) ? -1 : 0);

				if (!Content?.Size.Contains(contentPosition) ?? true)
					return Character.Plain('.');

				return Content[contentPosition];
			}
		}

		protected override void Resize()
		{
			using (Freeze())
			{
				_contentContext.MinSize = MinSize.Shrink(
					(BorderPlacement.HasFlag(BorderPlacement.Left) ? 1 : 0) + (BorderPlacement.HasFlag(BorderPlacement.Right) ? 1 : 0),
					(BorderPlacement.HasFlag(BorderPlacement.Top) ? 1 : 0) + (BorderPlacement.HasFlag(BorderPlacement.Bottom) ? 1 : 0));

				_contentContext.MaxSize = MaxSize.Shrink(
					(BorderPlacement.HasFlag(BorderPlacement.Left) ? 1 : 0) + (BorderPlacement.HasFlag(BorderPlacement.Right) ? 1 : 0),
					(BorderPlacement.HasFlag(BorderPlacement.Top) ? 1 : 0) + (BorderPlacement.HasFlag(BorderPlacement.Bottom) ? 1 : 0));

				_contentContext?.NotifySizeChanged();

				Size = Size.Bound(
					MinSize,
					Content?.Size.Expand(
						(BorderPlacement.HasFlag(BorderPlacement.Left) ? 1 : 0) + (BorderPlacement.HasFlag(BorderPlacement.Right) ? 1 : 0),
						(BorderPlacement.HasFlag(BorderPlacement.Top) ? 1 : 0) + (BorderPlacement.HasFlag(BorderPlacement.Bottom) ? 1 : 0)
					) ?? Size.Empty,
					MaxSize);
			}
		}

		private class BorderContext : IDrawingContext
		{
			private Border _border;

			public BorderContext(Border border)
			{
				_border = border;
			}

			public Size MinSize { get; set; }
			public Size MaxSize { get; set; }

			public void Redraw(IControl control)
			{
				if (_border?.Content != control) return;

				_border?.Redraw();
			}

			public void Update(IControl control, in Position position)
			{
				if (_border?.Content != control) return;

				_border?.Update(position.Move(
					_border.BorderPlacement.HasFlag(BorderPlacement.Left) ? 1 : 0,
					_border.BorderPlacement.HasFlag(BorderPlacement.Top) ? 1 : 0));
			}

			public void NotifySizeChanged()
			{
				SizeLimitsChanged?.Invoke(this);
			}

			public event SizeLimitsChangedHandler SizeLimitsChanged;
		}
	}
}
