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
				.Set(ref _context, value)
				.OnSizeLimitsChange(OnSizeLimitsChanged);
		}

		private Size _size;
		public Size Size
		{
			get => _size;
			set => Setter
				.Set(ref _size, value)
				.Then(Update);
		}

		public Size MinSize => _context?.MinSize ?? Size.Empty;
		public Size MaxSize => _context?.MaxSize ?? Size.Empty;

		protected void Update()
		{
			if (_freezeCount == 0)
				Context?.Update(this);
			else
				_changedDuringFreeze = true;
		}

		protected void Update(Position position)
		{
			if (_freezeCount == 0)
				Context?.Update(this, position);
			else
				_changedDuringFreeze = true;
		}

		protected FreezeContext Freeze()
		{
			return new FreezeContext(this);
		}

		private void OnSizeLimitsChanged(IDrawingContext context)
		{
			if (context == _context)
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
				_control._context.Update(_control);
			}
		}
	}
}
