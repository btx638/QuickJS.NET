using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using QuickJS.Native;
using static QuickJS.Native.QuickJSNativeApi;

namespace QuickJS
{
	internal sealed class QuickJSSafeDelegate
	{
		private readonly JSCFunction _callback;
		private readonly Delegate _handler;

		public unsafe QuickJSSafeDelegate(JSCFunction function)
		{
			_callback = function;
			_handler = sizeof(JSValue) == sizeof(ulong) ? (Delegate)new JSCFunction32(Impl8) : new JSCFunction(Impl16);
		}

		private unsafe ulong Impl8(JSContext cx, JSValue thisArg, int argc, JSValue[] argv)
		{
			try
			{
				return _callback(cx, thisArg, argc, argv).uint64;
			}
			catch (Exception ex)
			{
				IntPtr opaque = JS_GetContextOpaque(cx);
				if (opaque != IntPtr.Zero)
					((QuickJSContext)GCHandle.FromIntPtr(opaque).Target).ClrException = ex;

				return JS_ThrowInternalError(cx, ex.Message).uint64;
			}
		}

		private unsafe JSValue Impl16(JSContext cx, JSValue thisArg, int argc, JSValue[] argv)
		{
			try
			{
				return _callback(cx, thisArg, argc, argv);
			}
			catch (Exception ex)
			{
				IntPtr opaque = JS_GetContextOpaque(cx);
				if (opaque != IntPtr.Zero)
					((QuickJSContext)GCHandle.FromIntPtr(opaque).Target).ClrException = ex;

				return JS_ThrowInternalError(cx, ex.Message);
			}
		}

		[MethodImpl(AggressiveInlining)]
		public IntPtr GetPointer()
		{
			return Marshal.GetFunctionPointerForDelegate(_handler);
		}

	}
}
