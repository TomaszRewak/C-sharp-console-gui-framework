using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleGUI.Controls
{
	public class WrapPanel : Control, IDrawingContextListener
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

		public override Character this[Position position]
		{
			get
			{
				var localPosition = position.UnWrap(Size.Width);

				if (ContentContext.Contains(localPosition))
					return ContentContext[localPosition];

				return Character.Empty;
			}
		}

		protected override void Initialize()
		{
			if (MaxSize.Width == 0)
			{
				Redraw();
				return;
			}

			using (Freeze())
			{
				ContentContext.SetLimits(
					new Size(0, 1),
					new Size(Math.Max(0, MaxSize.Width * MaxSize.Height), 1));

				Resize(new Size(Math.Min(ContentContext.Size.Width, MaxSize.Width), (ContentContext.Size.Width - 1) / MaxSize.Width + 1));
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
			if (Size.Width == 0)
			{
				Redraw();
				return;
			}

			var begin = rect.LeftTopCorner.Wrap(Size.Width);
			var end = rect.RightBottomCorner.Wrap(Size.Width);

			Update(new Rect(0, begin.Y, Size.Width, end.Y - begin.Y + 1));
		}
	}
}
