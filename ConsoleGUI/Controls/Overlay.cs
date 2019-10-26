using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Controls
{
	public class Overlay : Control, IDrawingContextListener
	{
		private DrawingContext _topContentContext = DrawingContext.Dummy;
		private DrawingContext TopContentContext
		{
			get => _topContentContext;
			set => Setter
				.SetDisposable(ref _topContentContext, value)
				.Then(Initialize);
		}

		private DrawingContext _bottomContentContext = DrawingContext.Dummy;
		private DrawingContext BottomContentContext
		{
			get => _bottomContentContext;
			set => Setter
				.SetDisposable(ref _bottomContentContext, value)
				.Then(Initialize);
		}

		private IControl _topContent;
		public IControl TopContent
		{
			get => _topContent;
			set => Setter
				.Set(ref _topContent, value)
				.Then(BindTopContent);
		}

		private IControl _bottomContent;
		public IControl BottomContent
		{
			get => _bottomContent;
			set => Setter
				.Set(ref _bottomContent, value)
				.Then(BindBottomContent);
		}

		public override Character this[Position position]
		{
			get
			{
				if (TopContentContext.Contains(position))
				{
					var character = TopContentContext[position];
					if (character != Character.Empty) return character;
				}

				if (BottomContentContext.Contains(position))
				{
					var character = BottomContentContext[position];
					if (character != Character.Empty) return character;
				}

				return Character.Empty;
			}
		}

		protected override void Initialize()
		{
			using (Freeze())
			{
				TopContentContext.SetLimits(MinSize, MaxSize);
				BottomContentContext.SetLimits(MinSize, MaxSize);

				Resize(Size.Max(TopContentContext.Size, BottomContentContext.Size));
			}
		}

		private void BindTopContent()
		{
			TopContentContext = new DrawingContext(this, TopContent);
		}

		private void BindBottomContent()
		{
			BottomContentContext = new DrawingContext(this, BottomContent);
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
