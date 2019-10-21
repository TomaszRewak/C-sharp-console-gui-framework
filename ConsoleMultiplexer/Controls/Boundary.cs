using ConsoleMultiplexer.Common;
using ConsoleMultiplexer.Data;
using ConsoleMultiplexer.Space;
using ConsoleMultiplexer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class Boundary : Control, IDrawingContextListener
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

		private Size? _minContentSize;
		public Size? MinContentSize
		{
			get => _minContentSize;
			set => Setter
				.Set(ref _minContentSize, value)
				.Then(Initialize);
		}

		private Size? _maxContentSize;
		public Size? MaxContentSize
		{
			get => _maxContentSize;
			set => Setter
				.Set(ref _maxContentSize, value)
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

				Resize(Size.Clip(Size.Empty, ContentContext.Size, Size.Infinite));
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
