using System;
using System.Runtime.InteropServices;

namespace QuickJS
{
	internal static class NativeMethods
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string procName);

		[DllImport("libdl", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
		public static extern IntPtr dlopen(string path, int mode);

		[DllImport("libdl", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
		public static extern int dlclose(IntPtr handle);
	}
}
