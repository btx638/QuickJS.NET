using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	[StructLayout(LayoutKind.Sequential)]
	public struct JSPropertyEnum
	{
		[MarshalAs(UnmanagedType.Bool)]
		public bool is_enumerable;
		public JSAtom atom;
	}


}
