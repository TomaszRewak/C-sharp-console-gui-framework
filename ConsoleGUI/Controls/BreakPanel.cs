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
	public class BreakPanel : Control, IDrawingContextListener
	{
		private readonly VerticalStackPanel _stackPanel;
		private readonly DrawingContext _stackPanelContext;
		private readonly List<WrapPanel> _wrapPanels;

		public BreakPanel()
		{
			_stackPanel = new VerticalStackPanel();
			_stackPanelContext = new DrawingContext(this, _stackPanel);
			_wrapPanels = new List<WrapPanel>();
		}

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

		public override Cell this[Position position]
		{
			get
			{
				return _stackPanel[position];
			}
		}

		protected override void Initialize()
		{
			using (Freeze())
			{
				ContentContext.SetLimits(
					new Size(0, 1),
					new Size(Math.Max(0, MaxSize.Width * MaxSize.Height), 1));

				_stackPanelContext.SetLimits(MinSize, MaxSize);

				int breaks = 0;
				int width = 0;

				for (int x = 0; x <= Content.Size.Width; x++)
				{
					if (Content[new Position(x, 0)].Character.Content == '\n' || x == Content.Size.Width)
					{
						if (_wrapPanels.Count <= breaks)
						{
							var newWrapPanel = new WrapPanel() { Content = new DrawingSection() };
							_stackPanel.Add(newWrapPanel);
							_wrapPanels.Add(newWrapPanel);
						}

						var drawingSection = _wrapPanels[breaks].Content as DrawingSection;
						drawingSection.Content = Content;
						drawingSection.Rect = new Rect(x - width, 0, width + 1, 1);

						breaks++;
						width = 0;
					}
					else
					{
						width++;
					}
				}

				while (_wrapPanels.Count > breaks)
				{
					_stackPanel.Remove(_wrapPanels.Last());
					_wrapPanels.RemoveAt(_wrapPanels.Count - 1);
				}

				Resize(_stackPanel.Size);
			}
		}

		private void BindContent()
		{
			ContentContext = new DrawingContext(this, Content);
		}

		void IDrawingContextListener.OnRedraw(DrawingContext drawingContext)
		{
			if (drawingContext == ContentContext)
				Initialize();
			if (drawingContext == _stackPanelContext)
				Resize(_stackPanelContext.Size);
		}

		void IDrawingContextListener.OnUpdate(DrawingContext drawingContext, Rect rect)
		{
			if (drawingContext == ContentContext)
			{
				using (Freeze())
				{
					Initialize();
					foreach (var wrapPanel in _wrapPanels)
						(wrapPanel.Content as DrawingSection).Update(rect);
				}
			}

			if (drawingContext == _stackPanelContext)
				Update(rect);
		}
	}
}
