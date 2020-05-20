using System;
using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	/// <summary>
	/// Represents a pointer to a native JSModuleDef
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct JSModuleDef
	{
		private unsafe void* _moduledef;

		/// <inheritdoc/>
		public unsafe override int GetHashCode()
		{
			return new IntPtr(_moduledef).GetHashCode();
		}

		/// <inheritdoc/>
		public unsafe override bool Equals(object obj)
		{
			if (obj is JSModuleDef a)
				return a._moduledef == _moduledef;
			return false;
		}

		/// <summary>
		/// Converts the value of this instance to a pointer to a an
		/// unspecified type.
		/// </summary>
		/// <returns>
		/// A pointer to <see cref="void"/>; that is, a pointer to memory
		/// containing data of an unspecified type.
		/// </returns>
		public unsafe void* ToPointer()
		{
			return _moduledef;
		}

	}
}
