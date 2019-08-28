using ConsoleMultiplexer.DataStructures;
using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public sealed class Border : Control
	{
		private class BorderContext : IDrawingContext, IDisposable
		{
			private Border _border;

			public BorderContext(Border border)
			{
				_border = border;
			}

			private Size _size;
			public Size Size
			{
				get => Size;
				set => Setter
					.Set(ref _size, value)
					.Then(SizeChanged);
			}

			public Size MinSize => Size;
			public Size MaxSize => Size;

			public void Dispose()
			{
				_border = null;
			}

			public void Update(IControl control)
			{
				if (_border?.Content != control) return;

				_border?.Update();
			}

			public void Update(IControl control, in Position position)
			{
				if (_border?.Content != control) return;

				_border?.Update(position.Move(
					_border.BorderPlacement.HasFlag(BorderPlacement.Left) ? 1 : 0,
					_border.BorderPlacement.HasFlag(BorderPlacement.Top) ? 1 : 0));
			}

			private void SizeChanged()
			{
				SizeLimitsChanged?.Invoke(this);
			}

			public event SizeLimitsChangedHandler SizeLimitsChanged;
		}

		private IControl _content;
		public IControl Content
		{
			get => _content;
			set => Setter
				.Set(ref _content, value)
				.Then(UpdateContext);
		}

		private BorderContext _contentContext;
		private BorderContext ContentContext
		{
			get => _contentContext;
			set => Setter
				.Set(ref _contentContext, value)
				.DisposeOld();
		}

		private BorderPlacement _borderPlacement = BorderPlacement.All;
		public BorderPlacement BorderPlacement
		{
			get => _borderPlacement;
			set => Setter
				.Set(ref _borderPlacement, value)
				.Then(UpdateContext);
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

		protected override void Resize(Size MinSize, Size MaxSize)
		{
			using(Freeze())
			{
				Size = Size.Limit(MinSize, Content?.Size ?? Size.Empty, MaxSize);

				if (ContentContext != null)
					ContentContext.Size = Size.Shrink(
						(BorderPlacement.HasFlag(BorderPlacement.Left) ? 1 : 0) + (BorderPlacement.HasFlag(BorderPlacement.Right) ? 1 : 0),
						(BorderPlacement.HasFlag(BorderPlacement.Top) ? 1 : 0) + (BorderPlacement.HasFlag(BorderPlacement.Bottom) ? 1 : 0));
			}
		}

		private void UpdateContext()
		{
			Content.Context = ContentContext = new BorderContext(this);
		}

		private event SizeLimitsChangedHandler SizeLimitsChanged;
	}
}
