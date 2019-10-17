using ConsoleMultiplexer.Common;
using ConsoleMultiplexer.Space;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Test.Common
{
	[TestFixture]
	public class DrawingContextTest
	{
		[Test]
		public void DummyDrawingContext_HasEmptySize()
		{
			var drawingContext = DrawingContext.Dummy;

			Assert.AreEqual(Size.Empty, drawingContext.Size);
		}
	}
}
