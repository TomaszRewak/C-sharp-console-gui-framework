using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Helpers
{
	internal class Setter
	{
		public static SetterContext<T> Set<T>(ref T field, T value)
		{
			var different = !Equals(field, value);

			if (different)
				field = value;

			return new SetterContext<T>(different);
		}
	}

	internal struct SetterContext<T>
	{
		public bool Changed { get; }

		public SetterContext(bool changed)
		{
			Changed = changed;
		}

		public void Then(Action action)
		{
			if (Changed)
				action();
		}
	}
}
