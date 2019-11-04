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
	public sealed class Button : Control, IDrawingContextListener, IMouseListener
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

		private Color _mouseOverColor = new Color(50, 50, 100);
		public Color MouseOverColor
		{
			get => _mouseOverColor;
			set => Setter
				.Set(ref _mouseOverColor, value)
				.Then(Redraw);
		}

		private Color _mouseDownColor = new Color(50, 50, 200);
		public Color MouseDownColor
		{
			get => _mouseDownColor;
			set => Setter
				.Set(ref _mouseDownColor, value)
				.Then(Redraw);
		}

		private bool _mouseOver;
		private bool MouseOver
		{
			get => _mouseOver;
			set => Setter
				.Set(ref _mouseOver, value)
				.Then(Redraw);
		}

		private bool _mouseDown;
		private bool MouseDown
		{
			get => _mouseDown;
			set => Setter
				.Set(ref _mouseDown, value)
				.Then(Redraw);
		}

		public override Cell this[Position position]
		{
			get
			{
				var character = ContentContext[position];

				if (MouseDown)
					character = character.WithBackground(character.Background?.Mix(MouseDownColor, 0.75f) ?? MouseDownColor);
				else if (MouseOver)
					character = character.WithBackground(character.Background?.Mix(MouseOverColor, 0.75f) ?? MouseOverColor);

				return character.WithMouseListener(this, position);
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
			ContentContext = new DrawingContext(this, _content);
		}

		void IDrawingContextListener.OnRedraw(DrawingContext drawingContext)
		{
			Initialize();
		}

		void IDrawingContextListener.OnUpdate(DrawingContext drawingContext, Rect rect)
		{
			Update(rect);
		}

		void IMouseListener.OnMouseEnter()
		{
			MouseOver = true;
		}

		void IMouseListener.OnMouseLeave()
		{
			MouseOver = false;
			MouseDown = false;
		}

		void IMouseListener.OnMouseUp(Position position)
		{
			MouseDown = false;
		}

		void IMouseListener.OnMouseDown(Position position)
		{
			MouseDown = true;
		}

		void IMouseListener.OnMouseMove(Position position)
		{ }
	}
}
