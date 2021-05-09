using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;

namespace ConsoleGUI.Controls
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

		private BorderStyle _borderStyle = BorderStyle.Double;
		public BorderStyle BorderStyle
		{
			get => _borderStyle;
			set => Setter
				.Set(ref _borderStyle, value)
				.Then(Redraw);
		}

		public override Cell this[Position position]
		{
			get
			{
				if (ContentContext.Contains(position))
					return ContentContext[position];

				if (position.X == 0 && position.Y == 0 && BorderPlacement.HasBorder(BorderPlacement.Top | BorderPlacement.Left))
					return _borderStyle.TopLeft;

				if (position.X == Size.Width - 1 && position.Y == 0 && BorderPlacement.HasBorder(BorderPlacement.Top | BorderPlacement.Right))
					return _borderStyle.TopRight;

				if (position.X == 0 && position.Y == Size.Height - 1 && BorderPlacement.HasBorder(BorderPlacement.Bottom | BorderPlacement.Left))
					return _borderStyle.BottomLeft;

				if (position.X == Size.Width - 1 && position.Y == Size.Height - 1 && BorderPlacement.HasBorder(BorderPlacement.Bottom | BorderPlacement.Right))
					return _borderStyle.BottomRight;

				if (position.X == 0 && BorderPlacement.HasBorder(BorderPlacement.Left))
					return _borderStyle.Left;

				if (position.X == Size.Width - 1 && BorderPlacement.HasBorder(BorderPlacement.Right))
					return _borderStyle.Right;

				if (position.Y == 0 && BorderPlacement.HasBorder(BorderPlacement.Top))
					return _borderStyle.Top;

				if (position.Y == Size.Height - 1 && BorderPlacement.HasBorder(BorderPlacement.Bottom))
					return _borderStyle.Bottom;

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
