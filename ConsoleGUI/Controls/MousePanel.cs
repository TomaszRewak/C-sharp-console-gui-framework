using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Controls
{
	public sealed class MousePanel : Control, IDrawingContextListener, IMouseListener
	{
		public event EventHandler<Position> MouseMove;
		public event EventHandler<Position> MouseUp;
		public event EventHandler<Position> MouseDown;
		public event EventHandler MouseEnter;
		public event EventHandler MouseLeave;

		private IControl _content;
		public IControl Content
		{
			get => _content;
			set => Setter
				.Set(ref _content, value)
				.Then(BindContent);
		}

		private DrawingContext _contentContext = DrawingContext.Dummy;
		public DrawingContext ContentContext
		{
			get => _contentContext;
			set => Setter
				.SetDisposable(ref _contentContext, value)
				.Then(Initialize);
		}

		private bool _interceptChildEvents;
		public bool InterceptChildEvents
		{
			get => _interceptChildEvents;
			set => Setter
				.Set(ref _interceptChildEvents, value)
				.Then(Redraw);
		}

		private Position? _mousePosition;
		public Position? MousePosition
		{
			get => _mousePosition;
			private set => Setter
				.Set(ref _mousePosition, value);
		}

		private bool _isMouseDown;
		public bool IsMouseDown
		{
			get => _isMouseDown;
			private set => Setter
				.Set(ref _isMouseDown, value);
		}

		public bool IsMouseOver => MousePosition.HasValue;

		public override Cell this[Position position]
		{
			get
			{
				var cell = ContentContext[position];

				if (InterceptChildEvents || !cell.MouseListener.HasValue)
					cell = cell.WithMouseListener(this, position);

				return cell;
			}
		}

		protected override void Initialize()
		{
			using (Freeze())
			{
				ContentContext.SetLimits(MinSize, MaxSize);
				Resize(ContentContext.Size);
			}
		}

		private void BindContent()
		{
			this.ContentContext = new DrawingContext(this, Content);
		}

		void IDrawingContextListener.OnRedraw(DrawingContext drawingContext)
		{
			Initialize();
		}

		void IDrawingContextListener.OnUpdate(DrawingContext drawingContext, Rect rect)
		{
			Update(rect);
		}

		public void OnMouseDown(Position position)
		{
			IsMouseDown = true;
			MouseDown?.Invoke(this, position);
		}

		void IMouseListener.OnMouseEnter()
		{
			MouseEnter?.Invoke(this, EventArgs.Empty);
		}

		void IMouseListener.OnMouseLeave()
		{
			IsMouseDown = false;
			MousePosition = null;
			MouseLeave?.Invoke(this, EventArgs.Empty);
		}

		void IMouseListener.OnMouseMove(Position position)
		{
			MousePosition = position;
			MouseMove?.Invoke(this, position);
		}

		void IMouseListener.OnMouseUp(Position position)
		{
			IsMouseDown = false;
			MouseUp?.Invoke(this, position);
		}
	}
}
