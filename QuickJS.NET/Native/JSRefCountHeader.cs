using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	[StructLayout(LayoutKind.Sequential)]
	public struct JSRefCountHeader
	{
		public int ref_count;
	}


}
