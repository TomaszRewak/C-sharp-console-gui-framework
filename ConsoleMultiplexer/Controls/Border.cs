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
				.Then(BindContent);
		}

		private BorderPlacement _borderPlacement = BorderPlacement.All;
		public BorderPlacement BorderPlacement
		{
			get => _borderPlacement;
			set => Setter
				.Set(ref _borderPlacement, value)
				.Then(Resize);
		}

		private Color? _borderColor;
		public Color? BorderColor
		{
			get => _borderColor;
			set => Setter
				.Set(ref _borderColor, value)
				.Then(Redraw);
		}

		public override Character this[Position position]
		{
			get
			{
				if (!Size.Contains(position)) throw new IndexOutOfRangeException(nameof(position));

				if (position.X == 0 && position.Y == 0 && BorderPlacement.HasBorder(BorderPlacement.Top | BorderPlacement.Left))
					return new Character('╔', BorderColor);

				if (position.X == Size.Width - 1 && position.Y == 0 && BorderPlacement.HasBorder(BorderPlacement.Top | BorderPlacement.Right))
					return new Character('╗', BorderColor);

				if (position.X == 0 && position.Y == Size.Height - 1 && BorderPlacement.HasBorder(BorderPlacement.Bottom | BorderPlacement.Left))
					return new Character('╚', BorderColor);

				if (position.X == Size.Width - 1 && position.Y == Size.Height - 1 && BorderPlacement.HasBorder(BorderPlacement.Bottom | BorderPlacement.Right))
					return new Character('╝', BorderColor);

				if (position.X == 0 && BorderPlacement.HasBorder(BorderPlacement.Left))
					return new Character('║', BorderColor);

				if (position.X == Size.Width - 1 && BorderPlacement.HasBorder(BorderPlacement.Right))
					return new Character('║', BorderColor);

				if (position.Y == 0 && BorderPlacement.HasBorder(BorderPlacement.Top))
					return new Character('═', BorderColor);

				if (position.Y == Size.Height - 1 && BorderPlacement.HasBorder(BorderPlacement.Bottom))
					return new Character('═', BorderColor);

				var contentPosition = position.Move(
					BorderPlacement.HasBorder(BorderPlacement.Left) ? -1 : 0,
					BorderPlacement.HasBorder(BorderPlacement.Top) ? -1 : 0);

				if (!Content?.Size.Contains(contentPosition) ?? true)
					return Character.Plain('.');

				return Content[contentPosition];
			}
		}

		protected override void Resize()
		{
			using (Freeze())
			{
				_contentContext.MinSize = MinSize.AsRect().Remove(BorderPlacement.AsOffset()).Size;
				_contentContext.MaxSize = MaxSize.AsRect().Remove(BorderPlacement.AsOffset()).Size;

				_contentContext?.NotifySizeChanged();

				var newSize = Content?.Size.AsRect().Add(BorderPlacement.AsOffset()).Size ?? Size.Empty;

				Redraw(newSize);
			}
		}

		private void BindContent()
		{
			if (Content == null) return;
			Content.Context = _contentContext;
		}

		private class BorderContext : IDrawingContext
		{
			private readonly Border _border;

			public BorderContext(Border border)
			{
				_border = border;
			}

			public Size MinSize { get; set; }
			public Size MaxSize { get; set; }

			public void Redraw(IControl control)
			{
				if (_border.Content != control) return;

				_border.Redraw(control.Size.AsRect().Add(_border.BorderPlacement.AsOffset()).Size);
			}

			public void Update(IControl control, in Rect rect)
			{
				if (_border.Content != control) return;

				_border.Update(rect.Move(_border.BorderPlacement.AsVector()));
			}

			public void NotifySizeChanged()
			{
				SizeLimitsChanged?.Invoke(this);
			}

			public event SizeLimitsChangedHandler SizeLimitsChanged;
		}
	}
}
