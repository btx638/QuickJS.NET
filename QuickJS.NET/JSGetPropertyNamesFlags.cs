using System;
using QuickJS.Native;

namespace QuickJS
{
	/// <summary>
	/// JS_GPN_* flags.
	/// </summary>
	[Flags]
	public enum JSGetPropertyNamesFlags
	{
		/* JS_GPN_* */

		/// <summary>
		/// 
		/// </summary>
		StringMask = (1 << 0),

		/// <summary>
		/// 
		/// </summary>
		SymbolMask = (1 << 1),

		/// <summary>
		/// 
		/// </summary>
		PrivateMask = (1 << 2),

		/// <summary>
		/// Only include the enumerable properties.
		/// </summary>
		EnumOnly = (1 << 4),

		/// <summary>
		/// Set the <see cref="JSPropertyEnum.is_enumerable"/> field.
		/// </summary>
		SetEnum = (1 << 5),

	}
}
