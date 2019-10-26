using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Utils
{
	internal class Setter
	{
		public static SetterContext Set<T>(ref T field, T value)
		{
			var changed = !Equals(field, value);

			if (changed)
				field = value;

			return new SetterContext(changed);
		}

		public static SetterContext SetContext(ref IDrawingContext field, IDrawingContext value, SizeLimitsChangedHandler onSizeLimitsChanged)
		{
			var changed = !Equals(field, value);

			if (changed)
			{
				if (field != null) field.SizeLimitsChanged -= onSizeLimitsChanged;
				if (value != null) value.SizeLimitsChanged += onSizeLimitsChanged;

				field = value;
			}

			return new SetterContext(changed);
		}

		public static SetterContext SetDisposable<T>(ref T field, T value) where T : IDisposable
		{
			var changed = !Equals(field, value);

			if (changed)
			{
				field?.Dispose();

				field = value;
			}

			return new SetterContext(changed);
		}
	}

	internal struct SetterContext
	{
		public bool Changed { get; private set; }

		public SetterContext(bool changed)
		{
			Changed = changed;
		}

		public SetterContext Then(Action action)
		{
			if (Changed)
				action();

			return this;
		}
	}
}
