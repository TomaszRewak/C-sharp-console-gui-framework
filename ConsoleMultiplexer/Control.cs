using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	public abstract class Control : IControl
	{
		private int _freezeCount;
		private bool _changedDuringFreeze;

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
			if (_freezeCount == 0)
				Context?.Redraw(this);
			else
				_changedDuringFreeze = true;
		}

		protected void Redraw(in Size newSize)
		{
			if (Size != newSize)
			{
				Size = newSize;

			}
		}

		protected void Update(in Rect rect)
		{
			if (_freezeCount == 0)
				Context?.Update(this, rect);
			else
				_changedDuringFreeze = true;
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

			public FreezeContext(Control control)
			{
				_control = control;
				_control._freezeCount++;
			}

			public void Dispose()
			{
				_control._freezeCount--;

				if (_control._freezeCount > 0 || !_control._changedDuringFreeze) return;

				_control._changedDuringFreeze = false;
				_control._context?.Redraw(_control);
			}
		}
	}
}
