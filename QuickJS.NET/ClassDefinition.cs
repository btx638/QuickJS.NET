using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using QuickJS.Native;
using static QuickJS.Native.QuickJSNativeApi;

namespace QuickJS
{
	class ClassDefinition
	{
		private readonly Delegate _callImpl;
		private readonly JSClassGCMark _markImpl;
		private readonly JSClassFinalizer _finalizerImpl;


		public unsafe ClassDefinition(JSClassID id, JSClassCall call, JSClassGCMark gcMark, JSClassFinalizer finalizer)
		{
			if ((id.ToInt32() & 0xFFFF0000) != 0) // JSObject.class_id is 16 bit unsigned integer.
				throw new ArgumentOutOfRangeException(nameof(id));

			JS_NewClassID(ref id);
			this.ID = id;
			if (call != null)
			{
				_callImpl = sizeof(JSValue) == 8 ? (Delegate)new JSClassCall32(CallImpl8) : new JSClassCall(CallImpl16);
				this.Call = call;
			}
			if (gcMark != null)
			{
				_markImpl = GCMarkImpl;
				this.Mark = gcMark;
			}
			if (finalizer != null)
			{
				_finalizerImpl = FinalizerImpl;
				this.Finalizer = finalizer;
			}
		}

		public JSClassID ID { get; }

		/// <summary>
		/// If it is not null, the object is a function. If has the
		/// <see cref="JSCallFlags.Constructor"/> flag, the function is called
		/// as a constructor. In this case, &apos;this_val&apos; is new.target.
		/// A constructor call only happens if the object constructor bit is set
		/// (see <see cref="JS_SetConstructorBit"/>).
		/// </summary>
		public JSClassCall Call { get; }

		public JSClassGCMark Mark { get; }

		public JSClassFinalizer Finalizer { get; }

		public virtual void CopyToClassDef(ref JSClassDef classDef)
		{
			classDef.call = _callImpl is null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(_callImpl);
			classDef.gc_mark = _markImpl;
			classDef.finalizer = _finalizerImpl;
		}

		private unsafe JSValue CallImpl16(JSContext cx, JSValue func_obj, JSValue this_val, int argc, JSValue[] argv, JSCallFlags flags)
		{
			try
			{
				return Call(cx, func_obj, this_val, argc, argv, flags);
			}
			catch (OutOfMemoryException)
			{
				return JS_ThrowOutOfMemory(cx);
			}
			catch (Exception ex)
			{
				IntPtr opaque = JS_GetContextOpaque(cx);
				if (opaque != IntPtr.Zero)
					((QuickJSContext)GCHandle.FromIntPtr(opaque).Target).SetClrException(ex);

				fixed (byte* msg = Utils.StringToManagedUTF8(ex.Message.Replace("%", "%%")))
				{
					return JS_ThrowInternalError(cx, msg, __arglist());
				}
			}
		}

		private unsafe ulong CallImpl8(JSContext cx, JSValue func_obj, JSValue this_val, int argc, JSValue[] argv, JSCallFlags flags)
		{
			try
			{
				return Call(cx, func_obj, this_val, argc, argv, flags).uint64;
			}
			catch (OutOfMemoryException)
			{
				return JS_ThrowOutOfMemory(cx);
			}
			catch (Exception ex)
			{
				IntPtr opaque = JS_GetContextOpaque(cx);
				if (opaque != IntPtr.Zero)
					((QuickJSContext)GCHandle.FromIntPtr(opaque).Target).SetClrException(ex);

				fixed (byte* msg = Utils.StringToManagedUTF8(ex.Message.Replace("%", "%%")))
				{
					return JS_ThrowInternalError(cx, msg, __arglist()).uint64;
				}
			}
		}

		private void GCMarkImpl(JSRuntime rt, JSValue val, JS_MarkFunc mark_func)
		{
			try { Mark(rt, val, mark_func); } catch { }
		}

		private void FinalizerImpl(JSRuntime rt, JSValue val)
		{
			try { Finalizer(rt, val); } catch { }
		}


	}
}
