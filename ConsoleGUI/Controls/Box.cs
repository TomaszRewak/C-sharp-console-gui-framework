using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Controls
{
	public class Box : Control, IDrawingContextListener
	{
		public enum VerticalPlacement
		{
			Top,
			Center,
			Bottom,
			Stretch
		}

		public enum HorizontalPlacement
		{
			Left,
			Center,
			Right,
			Stretch
		}

		private HorizontalPlacement _horizontalContentPlacement = HorizontalPlacement.Center;
		public HorizontalPlacement HorizontalContentPlacement
		{
			get => _horizontalContentPlacement;
			set => Setter
				.Set(ref _horizontalContentPlacement, value)
				.Then(Initialize);
		}

		private VerticalPlacement _verticalContentPlacement = VerticalPlacement.Center;
		public VerticalPlacement VerticalContentPlacement
		{
			get => _verticalContentPlacement;
			set => Setter
				.Set(ref _verticalContentPlacement, value)
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

		private DrawingContext _contentContext = DrawingContext.Dummy;
		private DrawingContext ContentContext
		{
			get => _contentContext;
			set => Setter
				.SetDisposable(ref _contentContext, value)
				.Then(Initialize);
		}

		public override Character this[Position position]
		{
			get
			{
				if (ContentContext.Contains(position))
					return ContentContext[position];

				return Character.Empty;
			}
		}

		protected override void Initialize()
		{
			using (Freeze())
			{
				var minSize = new Size(
					HorizontalContentPlacement == HorizontalPlacement.Stretch ? MinSize.Width : 0,
					VerticalContentPlacement == VerticalPlacement.Stretch ? MinSize.Height : 0);
				
				var maxSize = new Size(
					HorizontalContentPlacement == HorizontalPlacement.Stretch ? MaxSize.Width : Size.MaxLength,
					VerticalContentPlacement == VerticalPlacement.Stretch ? MaxSize.Height : Size.MaxLength);

				ContentContext.SetLimits(minSize, maxSize);

				Resize(ContentContext.Size);

				int left = 0;
				int top = 0;

				switch (VerticalContentPlacement)
				{
					case VerticalPlacement.Stretch:
					case VerticalPlacement.Top:
						top = 0;
						break;
					case VerticalPlacement.Center:
						top = (Size.Height - ContentContext.Size.Height) / 2;
						break;
					case VerticalPlacement.Bottom:
						top = Size.Height - ContentContext.Size.Height;
						break;
				}

				switch (HorizontalContentPlacement)
				{
					case HorizontalPlacement.Stretch:
					case HorizontalPlacement.Left:
						left = 0;
						break;
					case HorizontalPlacement.Center:
						left = (Size.Width - ContentContext.Size.Width) / 2;
						break;
					case HorizontalPlacement.Right:
						left = Size.Width - ContentContext.Size.Width;
						break;
				}

				ContentContext.SetOffset(new Vector(left, top));
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
