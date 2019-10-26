using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Controls
{
	public class Background : Control, IDrawingContextListener
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

		private Color _color;
		public Color Color
		{
			get => _color;
			set => Setter
				.Set(ref _color, value)
				.Then(Redraw);
		}

		public bool _important;
		public bool Important
		{
			get => _important;
			set => Setter
				.Set(ref _important, value)
				.Then(Redraw);
		}

		public override Character this[Position position]
		{
			get
			{
				if (!ContentContext.Contains(position)) return new Character(Color);

				var character = ContentContext[position];

				if (!character.Background.HasValue || Important)
					character = character.WithBackground(Color);

				return character;
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
