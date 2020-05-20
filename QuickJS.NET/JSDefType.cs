using System;
using System.Collections.Generic;
using System.Text;

namespace QuickJS
{
	public enum JSDefType : byte
	{
		CFunc = 0,
		CGetSet = 1,
		CGetSetMagic = 2,
		PropString = 3,
		PropInt32 = 4,
		PropInt64 = 5,
		PropDouble = 6,
		PropUndefined = 7,
		Object = 8,
		Alias = 9,

	}
}
