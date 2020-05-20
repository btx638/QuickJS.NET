using System;
using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate int JSModuleInitFunc(JSContext ctx, JSModuleDef m);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate JSValue JSCFunction(JSContext ctx, [In] JSValue this_val, int argc, [In] JSValue* argv);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate JSValue JSCFunctionMagic(JSContext ctx, [In] JSValue this_val, int argc, [In] JSValue* argv, int magic);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate JSValue JSCFunctionData(JSContext ctx, [In] JSValue this_val, int argc, [In] JSValue* argv, int magic, JSValue* func_data);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void JS_MarkFunc(JSRuntime rt, IntPtr gp);

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
	public unsafe delegate JSValue JSJobFunc(JSContext ctx, int argc, JSValue* argv);


	/* is_handled = TRUE means that the rejection is handled */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate void JSHostPromiseRejectionTracker(JSContext ctx, [In] JSValue promise, [In] JSValue reason, [MarshalAs(UnmanagedType.Bool)] bool is_handled, void* opaque);
}
