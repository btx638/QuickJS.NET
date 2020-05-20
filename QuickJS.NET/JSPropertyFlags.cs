using System;

namespace QuickJS
{
	/// <summary>
	/// JS_PROP_* flags for object properties.
	/// </summary>
	[Flags]
	public enum JSPropertyFlags
	{
		/* JS_PROP_*  */

		/// <summary>
		/// A property descriptor may be changed and a property may be deleted
		/// from the corresponding object.
		/// </summary>
		Configurable = (1 << 0),

		/// <summary>
		/// A value associated with a property may be changed with an assignment
		/// operator.
		/// </summary>
		Writable = (1 << 1),

		/// <summary>
		/// A property is visible to JavaScript for...in and for each ... in loops.
		/// </summary>
		Enumerable = (1 << 2),

		/// <summary>
		/// Configurable | Writable | Enumerable
		/// </summary>
		CWE = (Configurable | Writable | Enumerable),

		/// <summary>
		/// Used internally in Arrays
		/// </summary>
		Length = (1 << 3),

		/// <summary>
		/// Mask for NORMAL, GETSET, VARREF, AUTOINIT
		/// </summary>
		TMASK = (3 << 4),

		/// <summary>
		/// None
		/// </summary>
		Normal = (0 << 4),

		/// <summary>
		/// 
		/// </summary>
		GetSet = (1 << 4),

		/// <summary>
		/// Used internally
		/// </summary>
		VarRef = (2 << 4),

		/// <summary>
		/// Used internally
		/// </summary>
		AutoInit = (3 << 4),


		#region flags for JS_DefineProperty

		/// <summary>
		/// 
		/// </summary>
		HasShift = 8,

		/// <summary>
		/// 
		/// </summary>
		HasConfigurable = (1 << 8),

		/// <summary>
		/// 
		/// </summary>
		HasWritable = (1 << 9),

		/// <summary>
		/// 
		/// </summary>
		HasEnumerable = (1 << 10),

		/// <summary>
		/// 
		/// </summary>
		HasGet = (1 << 11),

		/// <summary>
		/// 
		/// </summary>
		HasSet = (1 << 12),

		/// <summary>
		/// 
		/// </summary>
		HasValue = (1 << 13),
		
		#endregion


		/// <summary>
		/// Throw an exception if false would be returned (JS_DefineProperty/JS_SetProperty)
		/// </summary>
		Throw = (1 << 14),

		/// <summary>
		/// Throw an exception if false would be returned in strict mode (JS_SetProperty)
		/// </summary>
		ThrowStrict = (1 << 15),

		/// <summary>
		/// Internal use
		/// </summary>
		NoAdd = (1 << 16),

		/// <summary>
		/// Internal use
		/// </summary>
		NoExotic = (1 << 17),

	}


}
