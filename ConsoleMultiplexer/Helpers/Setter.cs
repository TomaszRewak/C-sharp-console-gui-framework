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
	}

	internal struct SetterContext<T>
	{
		public T OldValue { get; }
		public T NewValue { get; }

		public bool Changed => !Equals(OldValue, NewValue);

		public SetterContext(in T oldValue, in T newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}

		public SetterContext<T> Then(Action action)
		{
			if (Changed)
				action();

			return this;
		}
	}

	internal static class DrawingContextSetterExtension
	{
		public static SetterContext<IDrawingContext> OnSizeLimitsChange(this SetterContext<IDrawingContext> context, SizeLimitsChangedHandler sizeLimitsChanged)
		{
			if (!context.Changed)
				return context;

			if (context.OldValue != null)
				context.OldValue.SizeLimitsChanged -= sizeLimitsChanged;

			if (context.NewValue != null)
				context.NewValue.SizeLimitsChanged += sizeLimitsChanged;

			return context;
		}
	}
}
