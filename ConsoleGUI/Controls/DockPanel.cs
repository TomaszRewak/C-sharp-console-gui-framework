using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Controls
{
	public class DockPanel : Control, IDrawingContextListener
	{
		public enum DockedContorlPlacement
		{
			Top,
			Right,
			Botton,
			Left
		}

		private DockedContorlPlacement placement = DockedContorlPlacement.Top;
		public DockedContorlPlacement Placement
		{
			get => placement;
			set => Setter
				.Set(ref placement, value)
				.Then(Initialize);
		}

		private IControl dockedControl;
		public IControl DockedControl
		{
			get => dockedControl;
			set => Setter
				.Set(ref dockedControl, value)
				.Then(BindDockedControl);
		}

		private IControl fillingControl;
		public IControl FillingControl
		{
			get => fillingControl;
			set => Setter
				.Set(ref fillingControl, value)
				.Then(BindFillingControl);
		}

		private DrawingContext dockedDrawingContext = DrawingContext.Dummy;
		private DrawingContext DockedDrawingContext
		{
			get => dockedDrawingContext;
			set => Setter
				.SetDisposable(ref dockedDrawingContext, value)
				.Then(Initialize);
		}

		private DrawingContext fillingDrawingContext = DrawingContext.Dummy;
		private DrawingContext FillingDrawingContext
		{
			get => fillingDrawingContext;
			set => Setter
				.SetDisposable(ref fillingDrawingContext, value)
				.Then(Initialize);
		}

		public override Character this[Position position]
		{
			get
			{
				if (DockedDrawingContext.Contains(position))
					return DockedDrawingContext[position];

				if (FillingDrawingContext.Contains(position))
					return FillingDrawingContext[position];

				return Character.Empty;
			}
		}

		protected override void Initialize()
		{
			using (Freeze())
			{
				switch (Placement)
				{
					case DockedContorlPlacement.Top:
					case DockedContorlPlacement.Botton:
						DockedDrawingContext.SetLimits(MinSize.WithHeight(0), MaxSize);
						FillingDrawingContext.SetLimits(MinSize.Shrink(0, DockedDrawingContext.Size.Height), MaxSize.Shrink(0, DockedDrawingContext.Size.Height));
						Resize(new Size(Math.Max(DockedDrawingContext.Size.Width, FillingDrawingContext.Size.Width), DockedDrawingContext.Size.Height + FillingDrawingContext.Size.Height));
						break;
					case DockedContorlPlacement.Left:
					case DockedContorlPlacement.Right:
						DockedDrawingContext.SetLimits(MinSize.WithWidth(0), MaxSize);
						FillingDrawingContext.SetLimits(MinSize.Shrink(DockedDrawingContext.Size.Width, 0), MaxSize.Shrink(DockedDrawingContext.Size.Width, 0));
						Resize(new Size(DockedDrawingContext.Size.Width + FillingDrawingContext.Size.Width, Math.Max(DockedDrawingContext.Size.Height, FillingDrawingContext.Size.Height)));
						break;
				}

				switch (Placement)
				{
					case DockedContorlPlacement.Top:
						DockedDrawingContext.SetOffset(new Vector(0, 0));
						FillingDrawingContext.SetOffset(new Vector(0, DockedDrawingContext.Size.Height));
						break;
					case DockedContorlPlacement.Botton:
						DockedDrawingContext.SetOffset(new Vector(0, Size.Height - DockedDrawingContext.Size.Height));
						FillingDrawingContext.SetOffset(new Vector(0, 0));
						break;
					case DockedContorlPlacement.Left:
						DockedDrawingContext.SetOffset(new Vector(0, 0));
						FillingDrawingContext.SetOffset(new Vector(DockedDrawingContext.Size.Width, 0));
						break;
					case DockedContorlPlacement.Right:
						DockedDrawingContext.SetOffset(new Vector(Size.Width - DockedDrawingContext.Size.Width, 0));
						FillingDrawingContext.SetOffset(new Vector(0, 0));
						break;
				}
			}
		}

		private void BindDockedControl()
		{
			DockedDrawingContext = new DrawingContext(this, DockedControl);
		}

		private void BindFillingControl()
		{
			FillingDrawingContext = new DrawingContext(this, FillingControl);
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
