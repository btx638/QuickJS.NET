using System;
using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	/// <summary>
	/// Encapsulates a method works like the Proxy handler <see href="https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Proxy/Proxy/getOwnPropertyDescriptor">getOwnPropertyDescriptor</see>.
	/// </summary>
	/// <param name="ctx">The JavaScript context.</param>
	/// <param name="desc">A handle to the property descriptor.</param>
	/// <param name="obj">The target object.</param>
	/// <param name="atom">The identifier of the property whose description should be retrieved.</param>
	/// <returns>
	/// -1 if exception (can only happen in case of Proxy object); FALSE (0)
	/// if the property does not exists; TRUE (1) if it exists.
	/// If TRUE (1) is returned, the property descriptor <paramref name="desc"/>
	/// is filled if != NULL.
	/// </returns>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int JSGetOwnPropertyDelegate(JSContext ctx, JSPropertyDescriptorHandle desc, [In] JSValue obj, [In] JSAtom atom);

	/// <summary>
	/// Encapsulates a method works like the Proxy handler <see href="https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Proxy/Proxy/ownKeys">ownKeys</see>.
	/// </summary>
	/// <param name="cx">The JavaScript context.</param>
	/// <param name="ptab">A pointer to the C array that holds the property keys.</param>
	/// <param name="len">The number of elements contained in <paramref name="ptab"/>.</param>
	/// <param name="obj">The target object.</param>
	/// <returns>0 if OK, -1 if exception.</returns>
	/// <remarks>The &apos;is_enumerable&apos; field is ignored.</remarks>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate int JSGetOwnPropertyNamesUnsafeDelegate(JSContext cx, JSPropertyEnum** ptab, out int len, JSValue obj);

	/// <summary>
	/// Encapsulates a method works like the Proxy handler <see href="https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Proxy/Proxy/ownKeys">ownKeys</see>.
	/// </summary>
	/// <param name="cx">The JavaScript context.</param>
	/// <param name="keys">An array that holds the property keys.</param>
	/// <param name="obj">The target object.</param>
	/// <returns>0 if OK, -1 if exception.</returns>
	/// <remarks>The <see cref="JSPropertyEnum.is_enumerable"/> field is ignored.</remarks>
	public delegate int JSGetOwnPropertyNamesDelegate(JSContext cx, out JSPropertyEnum[] keys, JSValue obj);

	/// <summary>
	/// Encapsulates a method works like the Proxy handler <see href="https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Proxy/Proxy/deleteProperty">deleteProperty</see>.
	/// </summary>
	/// <param name="cx">The JavaScript context.</param>
	/// <param name="obj">The target object.</param>
	/// <param name="prop">The identifier of the property to delete.</param>
	/// <returns>-1 if exception, or TRUE (1) / FALSE (0).</returns>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int JSDeletePropertyDelegate(JSContext cx, JSValue obj, JSAtom prop);

	/// <summary>
	/// Encapsulates a method works like the Proxy handler <see href="https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Proxy/Proxy/defineProperty">defineProperty</see>.
	/// </summary>
	/// <param name="cx">The JavaScript context.</param>
	/// <param name="thisObj">The target object.</param>
	/// <param name="prop">The identifier of the property to define.</param>
	/// <param name="val">The value associated with the property.</param>
	/// <param name="getter">A function which serves as a getter for the property, or undefined if there is no getter.</param>
	/// <param name="setter">A function which serves as a setter for the property, or undefined if there is no setter.</param>
	/// <param name="flags">A bitwise combination of the <see cref="JSPropertyFlags"/>.</param>
	/// <returns>-1 if exception, or TRUE (1) / FALSE (0).</returns>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int JSDefineOwnPropertyDelegate(JSContext cx, JSValue thisObj, JSAtom prop, JSValue val, JSValue getter, JSValue setter, JSPropertyFlags flags);

	/// <summary>
	/// Encapsulates a method works like the Proxy handler <see href="https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Proxy/Proxy/has">has</see>.
	/// </summary>
	/// <param name="cx">The JavaScript context.</param>
	/// <param name="obj">The target object.</param>
	/// <param name="prop">The identifier of the property to check for existence.</param>
	/// <returns>-1 if exception, or TRUE (1) / FALSE (0).</returns>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int JSHasPropertyDelegate(JSContext cx, JSValue obj, JSAtom prop);

	/// <summary>
	/// Encapsulates a method works like the Proxy handler <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Proxy/Proxy/get">get</see>.
	/// </summary>
	/// <param name="cx">The JavaScript context.</param>
	/// <param name="obj">The target object.</param>
	/// <param name="prop">The identifier of the property to get.</param>
	/// <param name="receiver">Either the proxy or an object that inherits from the proxy.</param>
	/// <returns>Can return any valid <see cref="JSValue"/> value.</returns>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate JSValue JSGetPropertyDelegate(JSContext cx, JSValue obj, JSAtom prop, JSValue receiver);

	/// <summary>
	/// Encapsulates a method works like the Proxy handler <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Proxy/Proxy/get">get</see>.
	/// </summary>
	/// <param name="cx">The JavaScript context.</param>
	/// <param name="obj">The target object.</param>
	/// <param name="prop">The identifier of the property to get.</param>
	/// <param name="receiver">Either the proxy or an object that inherits from the proxy.</param>
	/// <returns>Can return any valid <see cref="JSValue"/> value.</returns>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate ulong JSGetPropertyDelegate32(JSContext cx, JSValue obj, JSAtom prop, JSValue receiver);

	/// <summary>
	/// Encapsulates a method works like the Proxy handler <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Proxy/Proxy/set">set</see>.
	/// </summary>
	/// <param name="cx">The JavaScript context.</param>
	/// <param name="obj">The target object.</param>
	/// <param name="prop">The identifier of the property to set.</param>
	/// <param name="value">The new value of the property to set.</param>
	/// <param name="receiver">
	/// The object to which the assignment was originally directed.
	/// This is usually the proxy itself. But a handler can also be called indirectly,
	/// via the prototype chain or various other ways.
	/// </param>
	/// <param name="flags">A bitwise combination of the <see cref="JSPropertyFlags"/>.</param>
	/// <returns>-1 if exception, or TRUE (1) / FALSE (0).</returns>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int JSSetPropertyDelegate(JSContext cx, JSValue obj, JSAtom prop, JSValue value, JSValue receiver, JSPropertyFlags flags);

	/// <summary>
	/// Contains the pointers for exotic behavior.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct JSClassExoticMethods
	{
		/// <summary>
		/// A trap for <see href="https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Object/getOwnPropertyDescriptor">Object.getOwnPropertyDescriptor()</see>.
		/// </summary>
		public JSGetOwnPropertyDelegate get_own_property;

		/// <summary>
		/// A trap for <see href="https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Object/getOwnPropertyNames">Object.getOwnPropertyNames()</see>.
		/// </summary>
		/// <remarks>The delegate type is <see cref="JSGetOwnPropertyNamesUnsafeDelegate"/>.</remarks>
		public Delegate get_own_property_names;

		/// <summary>
		/// A trap for the <see href="https://developer.mozilla.org/docs/Web/JavaScript/Reference/Operators/delete">delete</see> operator.
		/// </summary>
		public JSDeletePropertyDelegate delete_property;

		/// <summary>
		/// A trap for <see href="https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Object/defineProperty">Object.defineProperty()</see>.
		/// </summary>
		public JSDefineOwnPropertyDelegate define_own_property;

		/// <summary>
		/// A trap for the <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/in">in</see> operator. 
		/// </summary>
		public JSHasPropertyDelegate has_property;

		/// <summary>
		/// A trap for getting a property value.
		/// </summary>
		/// <remarks>The delegate type is <see cref="JSGetPropertyDelegate"/> or <see cref="JSGetPropertyDelegate32"/> (on a 32-bit OS).</remarks>
		public Delegate get_property;

		/// <summary>
		/// A trap for setting a property value.
		/// </summary>
		public JSSetPropertyDelegate set_property;
	}


}
