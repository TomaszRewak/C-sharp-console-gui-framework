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

		public bool Changed { get; }

		public SetterContext(T oldValue, T newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;

			Changed = !Equals(oldValue, newValue);
		}

		public void Then(Action action)
		{
			if (Changed)
				action();
		}
	}

	internal static class SetterContextExtension
	{
		public static SetterContext<IDrawingContext> OnSizeChanged(this SetterContext<IDrawingContext> context, SizeChanged onSizeChanged)
		{
			if (context.Changed)
			{
				if (context.OldValue != null)
					context.OldValue.SizeChanged -= onSizeChanged;
				if (context.NewValue != null)
					context.NewValue.SizeChanged += onSizeChanged;
			}

			return context;
		}
	}
}
