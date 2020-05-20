using System;
using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct JSMallocState
	{
		public UIntPtr malloc_count;
		public UIntPtr malloc_size;
		public UIntPtr malloc_limit;
		public void* opaque; /* user opaque */
	}


}
