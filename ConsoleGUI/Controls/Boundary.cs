using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Controls
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

		private int? _minWidth;
		public int? MinWidth
		{
			get => _minWidth;
			set => Setter
				.Set(ref _minWidth, value)
				.Then(Initialize);
		}

		private int? _minHeight;
		public int? MinHeight
		{
			get => _minHeight;
			set => Setter
				.Set(ref _minHeight, value)
				.Then(Initialize);
		}

		private int? _maxWidth;
		public int? MaxWidth
		{
			get => _maxWidth;
			set => Setter
				.Set(ref _maxWidth, value)
				.Then(Initialize);
		}

		private int? _maxHeight;
		public int? MaxHeight
		{
			get => _maxHeight;
			set => Setter
				.Set(ref _maxHeight, value)
				.Then(Initialize);
		}

		public int? Width
		{
			set
			{
				using (Freeze())
				{
					MinWidth = value;
					MaxWidth = value;
				}
			}
		}

		public int? Height
		{
			set
			{
				using (Freeze())
				{
					MinHeight = value;
					MaxHeight = value;
				}
			}
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
				var minSize = new Size(MinWidth ?? MinSize.Width, MinHeight ?? MinSize.Height);
				var maxSize = new Size(MaxWidth ?? MaxSize.Width, MaxHeight ?? MaxSize.Height);

				ContentContext.SetLimits(minSize, maxSize);

				Resize(Size.Clip(minSize, ContentContext.Size, maxSize));
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
