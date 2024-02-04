﻿using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Test.Controls
{
	[TestFixture]
	public class BorderTest
	{
		[Test]
		public void Border_AdjustsItsSize_ToTheContent()
		{
			var context = new Mock<IDrawingContext>();
			context.SetupGet(c => c.MinSize).Returns(new Size(0, 0));
			context.SetupGet(c => c.MaxSize).Returns(new Size(100, 100));

			var content = new Mock<IControl>();
			content.SetupGet(c => c.Size).Returns(new Size(20, 30));

			var border = new Border
			{
				Content = content.Object
			};
			(border as IControl).Context = context.Object;

			Assert.That(new Size(22, 32),Is.EqualTo( border.Size));
		}

		[Test]
		public void EmptyBorder_AdjustsItsSize_ToTheMinSizeOfItsContainer()
		{
			var context = new Mock<IDrawingContext>();
			context.SetupGet(c => c.MinSize).Returns(new Size(10, 20));
			context.SetupGet(c => c.MaxSize).Returns(new Size(40, 50));

			var border = new Border();
			(border as IControl).Context = context.Object;

			Assert.That(new Size(10, 20),Is.EqualTo( border.Size));
		}

		[Test]
		public void Border_ReturnsEmptyCharacter_ForItsInterior()
		{
			var context = new Mock<IDrawingContext>();
			context.SetupGet(c => c.MinSize).Returns(new Size(3, 3));
			context.SetupGet(c => c.MaxSize).Returns(new Size(3, 3));

			var border = new Border();
			(border as IControl).Context = context.Object;

			Assert.That(Character.Empty,Is.EqualTo(  border[new Position(1, 1)].Character));
		}

		[Test]
		public void Border_ReturnsEmptyCharacter_OutsideOfItsSize()
		{
			var context = new Mock<IDrawingContext>();
			context.SetupGet(c => c.MinSize).Returns(new Size(3, 3));
			context.SetupGet(c => c.MaxSize).Returns(new Size(3, 3));

			var border = new Border();
			(border as IControl).Context = context.Object;

			Assert.That(Character.Empty,Is.EqualTo( border[new Position(3, 1)].Character));
		}
	}
}
