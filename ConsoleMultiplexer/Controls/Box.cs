using ConsoleMultiplexer.Common;
using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Space;
using ConsoleMultiplexer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class Box : Control, IDrawingContextListener
	{
		public enum VerticalPlacement
		{
			Top,
			Center,
			Bottom
		}

		public enum HorizontalPlacement
		{
			Left,
			Center,
			Right
		}

		private HorizontalPlacement horizontalContentPlacement = HorizontalPlacement.Center;
		public HorizontalPlacement HorizontalContentPlacement
		{
			get => horizontalContentPlacement;
			set => Setter
				.Set(ref horizontalContentPlacement, value)
				.Then(Initialize);
		}

		private VerticalPlacement verticalContentPlacement = VerticalPlacement.Center;
		public VerticalPlacement VerticalContentPlacement
		{
			get => verticalContentPlacement;
			set => Setter
				.Set(ref verticalContentPlacement, value)
				.Then(Initialize);
		}

		private Size? minContentSize;
		public Size? MinContentSize
		{
			get => minContentSize;
			set => Setter
				.Set(ref minContentSize, value)
				.Then(Initialize);
		}

		private Size? maxContentSize;
		public Size? MaxContentSize
		{
			get => maxContentSize;
			set => Setter
				.Set(ref maxContentSize, value)
				.Then(Initialize);
		}

		private IControl content;
		public IControl Content
		{
			get => content;
			set => Setter
				.Set(ref content, value)
				.Then(BindContent);
		}

		private DrawingContext contentContext = DrawingContext.Dummy;
		private DrawingContext ContentContext
		{
			get => contentContext;
			set => Setter
				.SetDisposable(ref contentContext, value)
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
				ContentContext.SetLimits(
					MinContentSize ?? MinSize,
					MaxContentSize ?? MaxSize);

				Resize(ContentContext.Size);

				int left = 0;
				int top = 0;

				switch (VerticalContentPlacement)
				{
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
