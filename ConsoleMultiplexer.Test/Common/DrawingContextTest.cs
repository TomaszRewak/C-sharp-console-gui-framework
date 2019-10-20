using ConsoleMultiplexer.Common;
using ConsoleMultiplexer.Space;
using Moq;
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

		[Test]
		public void DrawingContext_PropagatesUpdates()
		{
			var listener = new Mock<IDrawingContextListener>();
			var control = new Mock<IControl>();

			var drawingContext = new DrawingContext(listener.Object, control.Object);
			drawingContext.SetLimits(new Size(10, 10), new Size(10, 10));

			listener.Reset();
			drawingContext.Update(control.Object, new Rect(1, 1, 5, 5));

			listener.Verify(l => l.OnUpdate(drawingContext, new Rect(1, 1, 5, 5)));
		}

		[Test]
		public void DrawingContext_PropagatesUpdates_WithOffset()
		{
			var listener = new Mock<IDrawingContextListener>();
			var control = new Mock<IControl>();

			var drawingContext = new DrawingContext(listener.Object, control.Object);
			drawingContext.SetLimits(new Size(10, 10), new Size(10, 10));
			drawingContext.SetOffset(new Vector(2, 2));

			listener.Reset();
			drawingContext.Update(control.Object, new Rect(1, 1, 5, 5));

			listener.Verify(l => l.OnUpdate(drawingContext, new Rect(3, 3, 5, 5)));
		}

		[Test]
		public void DrawingContext_UpdatesOldRect_AfterOffsetChage()
		{
			var listener = new Mock<IDrawingContextListener>();
			var control = new Mock<IControl>();
			control.SetupGet(c => c.Size).Returns(new Size(5, 5));

			var drawingContext = new DrawingContext(listener.Object, control.Object);
			drawingContext.SetLimits(new Size(10, 10), new Size(10, 10));
			drawingContext.SetOffset(new Vector(2, 2));

			listener.Reset();
			drawingContext.SetOffset(new Vector(4, 4));

			listener.Verify(l => l.OnUpdate(drawingContext, new Rect(2, 2, 5, 5)));
		}

		[Test]
		public void DrawingContext_PropagatesSizeLimits()
		{
			bool raised = false;

			var drawingContext = new DrawingContext(null, null);
			drawingContext.SizeLimitsChanged += c => raised |= c == drawingContext;

			drawingContext.SetLimits(new Size(10, 15), new Size(20, 25));

			Assert.IsTrue(raised);
		}

		[Test]
		public void DrawingContext_DoesntPropagateUpdates_WhenDisposed()
		{
			var listener = new Mock<IDrawingContextListener>();
			var control = new Mock<IControl>();

			var drawingContext = new DrawingContext(listener.Object, control.Object);
			drawingContext.Dispose();

			listener.Reset();
			drawingContext.Update(control.Object, new Rect(1, 1, 5, 5));

			listener.Verify(l => l.OnUpdate(It.IsAny<DrawingContext>(), It.IsAny<Rect>()), Times.Never);
		}
	}
}
