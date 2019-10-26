using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Utils
{
	internal struct FreezeLock
	{
		private int _freezeCount;

		public void Freeze()
		{
			_freezeCount++;
		}

		public void Unfreeze()
		{
			_freezeCount--;
		}

		public bool IsFrozen => _freezeCount > 0;
		public bool IsUnfrozen => !IsFrozen;
	}
}
