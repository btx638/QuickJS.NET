using System;
using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	[StructLayout(LayoutKind.Sequential)]
	public struct JSClassExoticMethods
	{
		/// <summary>
		/// Return -1 if exception (can only happen in case of Proxy object),
		/// FALSE if the property does not exists, TRUE if it exists. If 1 is
		/// returned, the property descriptor 'desc' is filled if != NULL.
		/// </summary>
		public IntPtr get_own_property;
		// int (* get_own_property) (JSContext* ctx, JSPropertyDescriptor* desc, JSValueConst obj, JSAtom prop);

		/// <summary>
		/// '*ptab' should hold the '*plen' property keys. Return 0 if OK, -1 if exception. The 'is_enumerable' field is ignored.
		/// </summary>
		public IntPtr get_own_property_names;
		//int (* get_own_property_names) (JSContext* ctx, JSPropertyEnum** ptab,	uint32_t* plen, JSValueConst obj);

		/// <summary>
		/// return &lt; 0 if exception, or TRUE/FALSE
		/// </summary>
		public IntPtr delete_property;
		// int (* delete_property) (JSContext* ctx, JSValueConst obj, JSAtom prop);

		/// <summary>
		/// return &lt; 0 if exception or TRUE/FALSE
		/// </summary>
		public IntPtr define_own_property;
		// int (* define_own_property) (JSContext* ctx, JSValueConst this_obj, JSAtom prop, JSValueConst val, JSValueConst getter, JSValueConst setter, int flags);

		/// <summary>
		/// return &lt 0 if exception or TRUE/FALSE
		/// </summary>
		public IntPtr has_property;
		// int (* has_property) (JSContext* ctx, JSValueConst obj, JSAtom atom);

		public IntPtr get_property;
		// JSValue(*get_property)(JSContext* ctx, JSValueConst obj, JSAtom atom, JSValueConst receiver);

		/// <summary>
		/// return &lt 0 if exception or TRUE/FALSE
		/// </summary>
		public IntPtr set_property;
		// int (* set_property) (JSContext* ctx, JSValueConst obj, JSAtom atom, JSValueConst value, JSValueConst receiver, int flags);
	}


}
