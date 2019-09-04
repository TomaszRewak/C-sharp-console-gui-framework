using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public abstract class Control : IControl
	{
		private int _freezeCount;
		private Rect _updatedRect;
		private Size _previousSize;

		public abstract Character this[Position position] { get; }
		protected abstract void Resize();

		private IDrawingContext _context;
		public IDrawingContext Context
		{
			private get => _context;
			set => Setter
				.SetContext(ref _context, value, OnSizeLimitsChanged)
				.Then(UpdateSizeLimits);
		}

		public Size Size { get; private set; }
		public Size MinSize { get; private set; }
		public Size MaxSize { get; private set; }

		protected void Redraw()
		{
			Redraw(Size);
		}

		protected void Redraw(in Size newSize)
		{
			using (Freeze())
			{
				Size = newSize;
				_updatedRect = Rect.OfSize(newSize);
			}
		}

		protected void Update(in Rect rect)
		{
			using (Freeze())
				_updatedRect = Rect.Surround(_updatedRect, rect);
		}

		protected FreezeContext Freeze()
		{
			return new FreezeContext(this);
		}

		private void OnSizeLimitsChanged(IDrawingContext context)
		{
			UpdateSizeLimits();
		}

		private void UpdateSizeLimits()
		{
			if (MinSize == _context?.MinSize && MaxSize == _context?.MaxSize) return;

			MinSize = _context?.MinSize ?? Size.Empty;
			MaxSize = _context?.MaxSize ?? Size.Empty;

			Resize();
		}

		protected struct FreezeContext : IDisposable
		{
			private readonly Control _control;

			private bool RequiresRedraw => _control.Size != _control._previousSize;
			private bool RequiresUpdate => !_control._updatedRect.IsEmpty;
			private bool IsUnfreezed => _control._freezeCount == 0;

			public FreezeContext(Control control)
			{
				_control = control;

				if (IsUnfreezed)
				{
					_control._previousSize = _control.Size;
					_control._updatedRect = Rect.Empty;
				}

				_control._freezeCount++;
			}

			public void Dispose()
			{
				_control._freezeCount--;

				if (!IsUnfreezed) return;

				if (RequiresRedraw)
					Redraw();
				else if (RequiresUpdate)
					Update();
			}

			private void Redraw() => _control.Context?.Redraw(_control);
			private void Update() => _control.Context.Update(_control, _control._updatedRect);
		}
	}
}
