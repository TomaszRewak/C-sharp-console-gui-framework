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

		public override Cell this[Position position]
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
					Math.Min(MinWidth ?? MinSize.Width, MaxWidth ?? int.MaxValue), 
					Math.Min(MinHeight ?? MinSize.Height, MaxHeight ?? int.MaxValue));
				var maxSize = new Size(
					Math.Max(MaxWidth ?? MaxSize.Width, MinWidth ?? 0), 
					Math.Max(MaxHeight ?? MaxSize.Height, MinHeight ?? 0));

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
