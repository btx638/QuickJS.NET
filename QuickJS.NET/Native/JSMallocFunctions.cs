using System;
using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	[StructLayout(LayoutKind.Sequential)]
	public struct JSMallocFunctions
	{
		//void* (* js_malloc) (JSMallocState* s, IntPtr size);
		//  void (* js_free) (JSMallocState* s, void* ptr);
		//  void* (* js_realloc) (JSMallocState* s, void* ptr, IntPtr size);
		//  size_t(*js_malloc_usable_size)(const void* ptr);
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public JSMallocDelegate js_malloc;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public JSFreeDelegate js_free;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public JSReallocDelegate js_realloc;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public JSMallocUsableSizeDelegate js_malloc_usable_size;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr JSMallocDelegate(ref JSMallocState s, UIntPtr size);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void JSFreeDelegate(ref JSMallocState s, IntPtr ptr);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr JSReallocDelegate(ref JSMallocState s, IntPtr ptr, UIntPtr size);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate UIntPtr JSMallocUsableSizeDelegate([In] IntPtr ptr);
	}


}
