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
			}
		}

		public void Remove(IControl control)
		{

			using (Freeze())
			{
				_children.RemoveAll(c => c.Control == control);

				Resize();
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
				int top = 0;
				foreach (var child in _children)
				{
					child.Top = top;

					child.MinSize = new Size(MaxSize.Width, 0);
					child.MaxSize = new Size(MaxSize.Width, Math.Max(0, MaxSize.Height - top));
					child.NotifySizeChanged();

					top += child.Height;
				}

				Redraw(new Size(MaxSize.Width, top));
			}
		}

		private class VerticalStackPanelContext : IDrawingContext
		{
			private readonly VerticalStackPanel _stackPanel;

			public VerticalStackPanelContext(VerticalStackPanel stackPanel, IControl control)
			{
				_stackPanel = stackPanel;
				Control = control;
				Control.Context = this;
			}

			public IControl Control { get; }
			public int Top { get; set; }
			public int Height => Control.Size.Height;

			public Size MinSize { get; set; }
			public Size MaxSize { get; set; }

			public void Redraw(IControl control)
			{
				if (control != Control) return;

				_stackPanel.Resize();
			}

			public void Update(IControl control, in Rect rect)
			{
				if (control != Control) return;

				_stackPanel.Update(rect.Move(new Vector(0, Top)));
			}

			public void NotifySizeChanged()
			{
				SizeLimitsChanged?.Invoke(this);
			}

			public event SizeLimitsChangedHandler SizeLimitsChanged;
		}
	}
}
