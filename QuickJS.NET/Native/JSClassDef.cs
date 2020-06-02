using System;
using System.Runtime.InteropServices;
using static QuickJS.Native.QuickJSNativeApi;

namespace QuickJS.Native
{
	/// <summary>
	/// Represents a method that will be invoked when an object is finalized (prepared for garbage collection).
	/// </summary>
	/// <param name="rt">The JS runtime in which garbage collection is taking place.</param>
	/// <param name="val">The object being destroyed.</param>
	/// <remarks>
	/// Calling JS code from a finalizer (e.g. with JS_Call) is not supported
	/// and will only lead to problems. Finalizers should only be used to free
	/// system resources and JS objects.
	/// </remarks>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void JSClassFinalizer(JSRuntime rt, JSValue val);

	/// <summary>
	/// Represents a method that provides the ability for the cycle removal algorithm to find the other objects referenced by this object.
	/// </summary>
	/// <param name="rt">The JavaScript runtime.</param>
	/// <param name="val">The JavaScript object.</param>
	/// <param name="mark_func">A method that QuickJS uses to mark objects.</param>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void JSClassGCMark(JSRuntime rt, [In] JSValue val, JS_MarkFunc mark_func);

	/// <summary>
	/// Represents a method that will be called when a script calls an object as though it were a function: &apos;obj()&apos;.
	/// </summary>
	/// <param name="ctx">The context in which the native function is being called.</param>
	/// <param name="func_obj">The function object.</param>
	/// <param name="this_val">The this argument.</param>
	/// <param name="argc">The number of arguments supplied to the function by the caller.</param>
	/// <param name="argv">The arguments.</param>
	/// <param name="flags">
	/// If has the <see cref="JSCallFlags.Constructor"/> flag, the function is called as a constructor.
	/// In this case, <paramref name="this_val"/> is new target. A constructor call only happens if the object
	/// constructor bit is set (see <see cref="JS_SetConstructorBit"/>).
	/// </param>
	/// <returns>
	/// On success, the callback must return any valid <see cref="JSValue"/>;
	/// otherwise it should either report an error and return the <see cref="JSValue.Exception"/>.
	/// </returns>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate JSValue JSClassCall(JSContext ctx, [In] JSValue func_obj, [In] JSValue this_val, int argc, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] JSValue[] argv, JSCallFlags flags);

	/// <summary>
	/// Represents a method that will be called when a script calls an object as though it were a function: &apos;obj()&apos;.
	/// </summary>
	/// <param name="ctx">The context in which the native function is being called.</param>
	/// <param name="func_obj">The function object.</param>
	/// <param name="this_val">The this argument.</param>
	/// <param name="argc">The number of arguments supplied to the function by the caller.</param>
	/// <param name="argv">The arguments.</param>
	/// <param name="flags">
	/// If has the <see cref="JSCallFlags.Constructor"/> flag, the function is called as a constructor.
	/// In this case, <paramref name="this_val"/> is new target. A constructor call only happens if the object
	/// constructor bit is set (see <see cref="JS_SetConstructorBit"/>).
	/// </param>
	/// <returns>
	/// On success, the callback must return any valid <see cref="JSValue"/>;
	/// otherwise it should either report an error and return the <see cref="JSValue.Exception"/>.
	/// </returns>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate ulong JSClassCall32(JSContext ctx, [In] JSValue func_obj, [In] JSValue this_val, int argc, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] JSValue[] argv, JSCallFlags flags);

	/// <summary>
	/// Describes a class of JavaScript objects.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct JSClassDef
	{
		/// <summary>
		/// Class name.
		/// </summary>
		public IntPtr class_name;

		/// <summary>
		/// The object finalizer hook.
		/// </summary>
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public JSClassFinalizer finalizer;

		/// <summary>
		/// The object GC mark hook.
		/// </summary>
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public JSClassGCMark gc_mark;

		/// <summary>
		/// A pointer to the <see cref="JSClassCall"/> (<see cref="JSClassCall32"/>)
		/// callback if the object of this class is a function.
		/// </summary>
		public IntPtr call;

		/// <summary>
		/// A pointer to the <see cref="JSClassExoticMethods"/> structure.
		/// Сan be NULL if none are present.
		/// </summary>
		/// <remarks>
		/// This structure must be alive while the runtime is alive.
		/// </remarks>
		public IntPtr exotic; // JSClassExoticMethods *exotic;
	}


}
