using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using QuickJS.Native;
using static QuickJS.Native.QuickJSNativeApi;

namespace QuickJS
{
	sealed unsafe class ExoticClassDefinition : ClassDefinition
	{
		private readonly JSClassExoticMethods _callbacks;
		private readonly JSClassExoticMethods _callbacksImpl;
		private IntPtr _exotic;

		public ExoticClassDefinition(JSClassID id, JSClassCall call, JSClassGCMark gcMark, JSClassFinalizer finalizer,
			JSGetOwnPropertyDelegate getOwnProperty, JSGetOwnPropertyNamesDelegate getOwnPropertyNames,
			JSDeletePropertyDelegate deleteProperty, JSDefineOwnPropertyDelegate defineOwnProperty,
			JSHasPropertyDelegate hasProperty, JSGetPropertyDelegate getProperty, JSSetPropertyDelegate setProperty)
			: base(id, call, gcMark, finalizer)
		{
			_callbacks.get_own_property = getOwnProperty;
			_callbacks.get_own_property_names = getOwnPropertyNames;
			_callbacks.delete_property = deleteProperty;
			_callbacks.define_own_property = defineOwnProperty;
			_callbacks.has_property = hasProperty;
			_callbacks.get_property = getProperty;
			_callbacks.set_property = setProperty;

			if (getOwnProperty != null)
				_callbacksImpl.get_own_property = GetOwnPropertyImpl;
			if (getOwnPropertyNames != null)
				_callbacksImpl.get_own_property_names = new JSGetOwnPropertyNamesUnsafeDelegate(GetOwnPropertyNamesImpl);
			if (deleteProperty != null)
				_callbacksImpl.delete_property = DeletePropertyImpl;
			if (defineOwnProperty != null)
				_callbacksImpl.define_own_property = DefineOwnPropertyImpl;
			if (hasProperty != null)
				_callbacksImpl.has_property = HasPropertyImpl;
			if (getProperty != null)
				_callbacksImpl.get_property = sizeof(JSValue) == 8 ? (Delegate)new JSGetPropertyDelegate32(GetPropertyImpl8) : new JSGetPropertyDelegate(GetPropertyImpl16);
			if (setProperty != null)
				_callbacksImpl.set_property = SetPropertyImpl;
			_exotic = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(JSClassExoticMethods)));
			Marshal.StructureToPtr(_callbacksImpl, _exotic, false);
		}

		~ExoticClassDefinition()
		{
			if (_exotic != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(_exotic);
				_exotic = IntPtr.Zero;
			}
		}

		public override void CopyToClassDef(ref JSClassDef classDef)
		{
			base.CopyToClassDef(ref classDef);
			classDef.exotic = _exotic;
		}

		private int GetOwnPropertyImpl(JSContext cx, JSPropertyDescriptorHandle desc, JSValue obj, JSAtom prop)
		{
			try
			{
				return _callbacks.get_own_property(cx, desc, obj, prop);
			}
			catch (OutOfMemoryException)
			{
				JS_ThrowOutOfMemory(cx);
				return -1;
			}
			catch (Exception ex)
			{
				Utils.ReportException(cx, ex);
				return -1;
			}
		}

		private int GetOwnPropertyNamesImpl(JSContext cx, JSPropertyEnum** props, out int len, JSValue obj)
		{
			len = 0;
			*props = null;
			JSPropertyEnum[] names = null;
			int error;
			try
			{
				error = ((JSGetOwnPropertyNamesDelegate)_callbacks.get_own_property_names)(cx, out names, obj);
				if (error == 0)
				{
					if (names != null && names.Length > 0)
					{
						*props = (JSPropertyEnum*)js_malloc(cx, sizeof(JSPropertyEnum) * names.Length);
						if (props == null)
						{
							FreePropEnum(cx, names, props);
							error = -1;
						}
						else
						{
							len = names.Length;
							for (int i = 0; i < names.Length; i++)
							{
								*(*props + i) = names[i];
							}
						}
					}
				}
			}
			catch (OutOfMemoryException)
			{
				FreePropEnum(cx, names, props);
				JS_ThrowOutOfMemory(cx);
				error = -1;
			}
			catch (Exception ex)
			{
				FreePropEnum(cx, names, props);
				Utils.ReportException(cx, ex);
				error = -1;
			}
			return error;
		}

		private int DeletePropertyImpl(JSContext cx, JSValue obj, JSAtom prop)
		{
			try
			{
				return _callbacks.delete_property(cx, obj, prop);
			}
			catch (OutOfMemoryException)
			{
				JS_ThrowOutOfMemory(cx);
				return -1;
			}
			catch (Exception ex)
			{
				Utils.ReportException(cx, ex);
				return -1;
			}
		}

		private int DefineOwnPropertyImpl(JSContext cx, JSValue thisObj, JSAtom prop, JSValue val, JSValue getter, JSValue setter, JSPropertyFlags flags)
		{
			try
			{
				return _callbacks.define_own_property(cx, thisObj, prop, val, getter, setter, flags);
			}
			catch (OutOfMemoryException)
			{
				JS_ThrowOutOfMemory(cx);
				return -1;
			}
			catch (Exception ex)
			{
				Utils.ReportException(cx, ex);
				return -1;
			}
		}

		private int HasPropertyImpl(JSContext cx, JSValue obj, JSAtom prop)
		{
			try
			{
				return _callbacks.has_property(cx, obj, prop);
			}
			catch (OutOfMemoryException)
			{
				JS_ThrowOutOfMemory(cx);
				return -1;
			}
			catch (Exception ex)
			{
				Utils.ReportException(cx, ex);
				return -1;
			}
		}

		private JSValue GetPropertyImpl16(JSContext cx, JSValue obj, JSAtom prop, JSValue receiver)
		{
			try
			{
				return ((JSGetPropertyDelegate)_callbacks.get_property)(cx, obj, prop, receiver);
			}
			catch (OutOfMemoryException)
			{
				return JS_ThrowOutOfMemory(cx);
			}
			catch (Exception ex)
			{
				return Utils.ReportException(cx, ex);
			}
		}

		private ulong GetPropertyImpl8(JSContext cx, JSValue obj, JSAtom prop, JSValue receiver)
		{
			try
			{
				return ((JSGetPropertyDelegate)_callbacks.get_property)(cx, obj, prop, receiver).uint64;
			}
			catch (OutOfMemoryException)
			{
				return JS_ThrowOutOfMemory(cx);
			}
			catch (Exception ex)
			{
				return Utils.ReportException(cx, ex);
			}
		}

		private int SetPropertyImpl(JSContext cx, JSValue obj, JSAtom prop, JSValue value, JSValue receiver, JSPropertyFlags flags)
		{
			try
			{
				return _callbacks.set_property(cx, obj, prop, value, receiver, flags);
			}
			catch (OutOfMemoryException)
			{
				JS_ThrowOutOfMemory(cx);
				return -1;
			}
			catch (Exception ex)
			{
				Utils.ReportException(cx, ex);
				return -1;
			}
		}

		private static void FreePropEnum(JSContext ctx, JSPropertyEnum[] props, JSPropertyEnum** ptab)
		{
			if (*ptab != null)
			{
				IntPtr mem = new IntPtr(*ptab);
				*ptab = null;
				js_free(ctx, mem);
			}
			if (props == null)
				return;

			for (int i = 0; i < props.Length; i++)
			{
				JS_FreeAtom(ctx, props[i].atom);
			}
		}

	}
}
