using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleMultiplexer.Controls
{


	public class VerticalStackPanel : Control
	{
		private readonly List<VerticalStackPanelContext> _children = new List<VerticalStackPanelContext>();

		public void Add(IControl control)
		{
			using(Freeze())
			{
				_children.Add(new VerticalStackPanelContext(this, control));

				Resize();
				Redraw();
			}
		}

		public void Remove(IControl control)
		{

			using (Freeze())
			{
				_children.RemoveAll(c => c.Control == control);

				Resize();
				Redraw();
			}
		}

		public override Character this[Position position]
		{
			get
			{
				foreach (var child in _children)
					if (position.Y < child.Top + child.Height)
						return child.Control[position.Move(0, -child.Top)];

				return Character.Empty;
			}
		}

		protected override void Resize()
		{
			using (Freeze())
			{
				Size = MaxSize;

				int top = 0;
				foreach (var child in _children)
				{
					child.Width = Size.Width;
					child.Top = top;

					top += child.Height;
				}
			}
		}

		private class VerticalStackPanelContext : IDrawingContext
		{
			private readonly VerticalStackPanel _stackPanel;

			public VerticalStackPanelContext(VerticalStackPanel stackPanel, IControl control)
			{
				_stackPanel = stackPanel;
				Control = control;
				Control.SetContext(this);
			}

			public IControl Control { get; }
			public int Top { get; set; }
			public int Height => Control.Size.Height;

			private int _width;
			public int Width
			{
				get => _width;
				set => Setter
					.Set(ref _width, value)
					.Then(NotifySizeChanged);
			}

			public Size MinSize => new Size(Width, 0);
			public Size MaxSize => new Size(Width, int.MaxValue);

			public void Redraw(IControl control)
			{
				if (control != Control) return;

				_stackPanel.Redraw();
			}

			public void Update(IControl control, in Position position)
			{
				if (control != Control) return;

				_stackPanel.Update(position.Move(0, Top));
			}

			private void NotifySizeChanged()
			{
				SizeLimitsChanged?.Invoke(this);
			}

			public event SizeLimitsChangedHandler SizeLimitsChanged;
		}
	}
}
