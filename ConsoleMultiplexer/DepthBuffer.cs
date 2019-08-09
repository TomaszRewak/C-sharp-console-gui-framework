using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer
{
	internal class DepthBuffer
	{
		private readonly WindowHandle[,] _depthData;

		public DepthBuffer(int width, int height)
		{
			_depthData = new WindowHandle[width, height];
		}
	}
}
