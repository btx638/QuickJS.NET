using System;

namespace QuickJS
{
	/// <summary>
	/// Object Writer/Reader flags.
	/// </summary>
	[Flags]
	public enum JSReaderWriterFlags
	{
		/// <summary>
		/// Allow function/module
		/// </summary>
		WriteObjBytecode = (1 << 0),
		/// <summary>
		/// Byte swapped output
		/// </summary>
		WriteObjBSwap = (1 << 1),
		/// <summary>
		/// Allow function/module
		/// </summary>
		ReadObjBytecode = (1 << 0),
		/// <summary>
		/// Avoid duplicating 'buf' data
		/// </summary>
		ReadObjRomData = (1 << 1) /*  */
	}


}
