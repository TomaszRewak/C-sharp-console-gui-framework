using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;

namespace ConsoleGUI.Common
{
	public abstract class Control : IControl
	{
		private FreezeLock _freezeLock;
		private Rect _updatedRect;
		private Size _previousSize;

		public abstract Cell this[Position position] { get; }

		protected abstract void Initialize();

		private IDrawingContext _context;
		IDrawingContext IControl.Context
		{
			get => _context;
			set => Setter
				.SetContext(ref _context, value, OnSizeLimitsChanged)
				.Then(UpdateSizeLimits);
		}

		public Size Size { get; private set; }
		protected Size MinSize { get; private set; }
		protected Size MaxSize { get; private set; }

		protected void Redraw()
		{
			Resize(Size);
		}

		protected void Resize(in Size newSize)
		{
			using (Freeze())
			{
				Size = Size.Clip(MinSize, newSize, MaxSize);
				_updatedRect = Rect.OfSize(Size);
			}
		}

		protected void Update(in Rect rect)
		{
			using (Freeze())
				_updatedRect = Rect.Surround(_updatedRect, rect);
		}

		protected internal FreezeContext Freeze()
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

			Initialize();
		}

		protected internal struct FreezeContext : IDisposable
		{
			private readonly Control _control;

			private bool RequiresRedraw => _control.Size != _control._previousSize;
			private bool RequiresUpdate => !_control._updatedRect.IsEmpty;

			public FreezeContext(Control control)
			{
				_control = control;

				if (_control._freezeLock.IsUnfrozen)
				{
					_control._previousSize = _control.Size;
					_control._updatedRect = Rect.Empty;
				}

				_control._freezeLock.Freeze();
			}

			public void Dispose()
			{
				_control._freezeLock.Unfreeze();

				if (_control._freezeLock.IsFrozen) return;

				if (RequiresRedraw)
					Redraw();
				else if (RequiresUpdate)
					Update();
			}

			private void Redraw() => _control._context?.Redraw(_control);
			private void Update() => _control._context?.Update(_control, _control._updatedRect);
		}
	}
}
