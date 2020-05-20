using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	[StructLayout(LayoutKind.Sequential)]
	public struct JSPropertyDescriptor
	{
		[MarshalAs(UnmanagedType.I4)]
		public JSPropertyFlags flags;
		public JSValue value;
		public JSValue getter;
		public JSValue setter;
	}


}
