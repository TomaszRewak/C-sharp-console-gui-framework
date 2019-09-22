using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public sealed class Border : Control, IDrawingContextListener
	{
		private DrawingContext _contentContext = DrawingContext.Dummy;
		private DrawingContext ContentContext
		{
			get => _contentContext;
			set => Setter
				.SetDisposable(ref _contentContext, value)
				.Then(Initialize);
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
				.Then(Initialize);
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
				//if (!Size.Contains(position)) throw new IndexOutOfRangeException(nameof(position));

				if (ContentContext.Contains(position))
					return ContentContext[position];

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

				return Character.Empty;
			}
		}

		protected override void Initialize()
		{
			using (Freeze())
			{
				ContentContext?.SetOffset(BorderPlacement.AsVector());
				ContentContext?.SetLimits(
					MinSize.AsRect().Remove(BorderPlacement.AsOffset()).Size,
					MaxSize.AsRect().Remove(BorderPlacement.AsOffset()).Size);

				Resize(Content?.Size.AsRect().Add(BorderPlacement.AsOffset()).Size ?? Size.Empty);
			}
		}

		private void BindContent()
		{
			ContentContext = new DrawingContext(this, Content);
		}

		void IDrawingContextListener.OnRedraw(DrawingContext drawingContext)
		{
			Initialize();
		}

		void IDrawingContextListener.OnUpdate(DrawingContext drawingContext, Rect rect)
		{
			Update(rect);
		}
	}
}
