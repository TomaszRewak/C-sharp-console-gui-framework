using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Helpers
{
	internal class Setter
	{
		public static SetterContext<T> Set<T>(ref T field, T value)
		{
			var context = new SetterContext<T>(field, value);

			if (context.Changed)
				field = value;

			return context;
		}

		public static SetterContext<IDrawingContext> SetContext(ref IDrawingContext field, IDrawingContext value, SizeLimitsChangedHandler onSizeLimitsChanged)
		{
			var context = new SetterContext<IDrawingContext>(field, value);

			if (context.Changed)
			{
				if (field != null) field.SizeLimitsChanged -= onSizeLimitsChanged;
				if (value != null) value.SizeLimitsChanged += onSizeLimitsChanged;

				field = value;
			}

			return context;
		}
	}

	internal struct SetterContext<T>
	{
		public bool Changed { get; private set; }

		public SetterContext(in T oldValue, in T newValue)
		{
			Changed = !Equals(oldValue, newValue);
		}

		public SetterContext<T> Then(Action action)
		{
			if (Changed)
				action();

			return this;
		}
	}
}
