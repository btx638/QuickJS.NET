using System;
using System.Runtime.InteropServices;
using static QuickJS.Native.QuickJSNativeApi;

namespace QuickJS.Native
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void JSClassFinalizer(JSRuntime rt, JSValue val);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void JSClassGCMark(JSRuntime rt, [In] JSValue val, [MarshalAs(UnmanagedType.FunctionPtr)] JS_MarkFunc mark_func);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate JSValue JSClassCall(JSContext ctx, [In] JSValue func_obj, [In] JSValue this_val, int argc, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] JSValue[] argv, JSCallFlags flags);

	[StructLayout(LayoutKind.Sequential)]
	public struct JSClassDef : IDisposable
	{
		private IntPtr class_name;

		[MarshalAs(UnmanagedType.FunctionPtr)]
		private JSClassFinalizer finalizer;

		[MarshalAs(UnmanagedType.FunctionPtr)]
		private JSClassGCMark gc_mark;

		[MarshalAs(UnmanagedType.FunctionPtr)]
		private JSClassCall call;

		public IntPtr exotic; // JSClassExoticMethods *exotic;


		public string ClassName
		{
			get
			{
				if (class_name == IntPtr.Zero)
					return null;
				return Marshal.PtrToStringAnsi(class_name);
			}
			set
			{
				Free();
				class_name = value != null ? Marshal.StringToHGlobalAnsi(value) : IntPtr.Zero;
			}
		}

		public JSClassFinalizer Finalizer
		{
			get { return finalizer; }
			set { finalizer = value; }
		}

		public JSClassGCMark GCMark
		{
			get { return gc_mark; }
			set { gc_mark = value; }
		}

		/// <summary>
		/// If it is not null, the object is a function. If has the
		/// <see cref="JSCallFlags.Constructor"/> flag, the function is called
		/// as a constructor. In this case, &apos;this_val&apos; is new.target.
		/// A constructor call only happens if the object constructor bit is set
		/// (see <see cref="JS_SetConstructorBit"/>).
		/// </summary>
		public JSClassCall Call
		{
			get { return call; }
			set { call = value; }
		}

		public IntPtr GetClassNameAddress()
		{
			return class_name;
		}

		public void Free()
		{
			if (class_name != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(class_name);
				class_name = IntPtr.Zero;
			}
		}

		public void Dispose()
		{
			Free();
		}
	}


}
