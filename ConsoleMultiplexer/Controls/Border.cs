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
					.Set
			}

			public Size MinSize => throw new NotImplementedException();
			public Size MaxSize => throw new NotImplementedException();

			public event SizeLimitsChangedHandler SizeLimitsChanged;

			public void Dispose()
			{
				_border = null;
			}

			public void Update()
			{
				throw new NotImplementedException();
			}

			public void Update(in Position position)
			{
				throw new NotImplementedException();
			}
		}


		public IControl Content;

		private BorderPlacement _borderPlacement;
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
					BorderPlacement.HasFlag(BorderPlacement.Left) ? 1 : 0,
					BorderPlacement.HasFlag(BorderPlacement.Top) ? 1 : 0);

				if (Content?.Size.Contains(contentPosition) ?? false)
					return Character.Empty;

				return Content[contentPosition];
			}
		}

		protected override void Resize(Size MinSize, Size MaxSize)
		{
			using(Freeze())
			{
				Size = Size.Limit(MinSize, Content?.Size ?? Size.Empty, MaxSize);
			}
		}

		private void UpdateContentSize

		private event SizeLimitsChangedHandler SizeLimitsChanged;
	}
}
