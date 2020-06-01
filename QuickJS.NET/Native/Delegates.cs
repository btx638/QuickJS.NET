using System;
using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate int JSModuleInitFunc(JSContext ctx, JSModuleDef m);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate JSValue JSCFunction(JSContext ctx, [In] JSValue this_val, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] JSValue[] argv);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate ulong JSCFunction32(JSContext ctx, [In] JSValue this_val, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] JSValue[] argv);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate JSValue JSCFunctionMagic(JSContext ctx, [In] JSValue this_val, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] JSValue[] argv, int magic);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate ulong JSCFunctionMagic32(JSContext ctx, [In] JSValue this_val, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] JSValue[] argv, int magic);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate JSValue JSCFunctionData(JSContext ctx, [In] JSValue this_val, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] JSValue[] argv, int magic, JSValue* func_data);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate ulong JSCFunctionData32(JSContext ctx, [In] JSValue this_val, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] JSValue[] argv, int magic, JSValue* func_data);

	/// <summary>
	/// Encapsulates a method that QuickJS uses to mark objects.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct JS_MarkFunc
	{
		private void* func;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _JS_MarkFunc(JSRuntime rt, IntPtr gp);

		/// <summary>
		/// Invokes the method represented by the current structure.
		/// </summary>
		/// <param name="rt">The JavaScript runtime.</param>
		/// <param name="gp">A pointer to the internal data of the object.</param>
		public void Invoke(JSRuntime rt, IntPtr gp)
		{
#if NETSTANDARD
			Marshal.GetDelegateForFunctionPointer<_JS_MarkFunc>(new IntPtr(func))(rt, gp);
#else
			((_JS_MarkFunc)Marshal.GetDelegateForFunctionPointer(new IntPtr(func), typeof(_JS_MarkFunc)))(rt, gp);
#endif
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void JSFreeArrayBufferDataFunc(JSRuntime rt, IntPtr opaque, IntPtr ptr);

	/// <summary>
	///  return != 0 if the JS code needs to be interrupted
	/// </summary>
	/// <param name="rt"></param>
	/// <param name="opaque"></param>
	/// <returns></returns>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int JSInterruptHandler(JSRuntime rt, IntPtr opaque);

	/* return the module specifier (allocated with js_malloc()) or NULL if
	   exception */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate IntPtr JSModuleNormalizeFunc(JSContext ctx, IntPtr module_base_name, [MarshalAs(UnmanagedType.LPStr)] string module_name, IntPtr opaque);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate JSModuleDef JSModuleLoaderFunc(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string module_name, IntPtr opaque);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate JSValue JSJobFunc(JSContext ctx, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] JSValue[] arg);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate ulong JSJobFunc32(JSContext ctx, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] JSValue[] arg);

	/* is_handled = TRUE means that the rejection is handled */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate void JSHostPromiseRejectionTracker(JSContext ctx, [In] JSValue promise, [In] JSValue reason, [MarshalAs(UnmanagedType.Bool)] bool is_handled, void* opaque);
}
