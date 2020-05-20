using System;
using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	/// <summary>
	/// Represents a pointer to a native JSRuntime
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct JSRuntime
	{
		private unsafe void* _runtime;

		/// <inheritdoc/>
		public unsafe override int GetHashCode()
		{
			return new IntPtr(_runtime).GetHashCode();
		}

		/// <inheritdoc/>
		public unsafe override bool Equals(object obj)
		{
			if (obj is JSRuntime a)
				return a._runtime == _runtime;
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
			return _runtime;
		}

		/// <summary>
		/// Compares two <see cref="JSRuntime"/> objects. The result specifies
		/// whether the values of the two <see cref="JSRuntime"/> objects are
		/// equal.
		/// </summary>
		/// <param name="left">A <see cref="JSRuntime"/> to compare.</param>
		/// <param name="right">A <see cref="JSRuntime"/> to compare.</param>
		/// <returns>
		/// true if <paramref name="left"/> and <paramref name="right"/> are
		/// equal; otherwise, false.
		/// </returns>
		public static unsafe bool operator ==(JSRuntime left, JSRuntime right)
		{
			return left._runtime == right._runtime;
		}

		/// <summary>
		/// Compares two <see cref="JSRuntime"/> objects. The result specifies
		/// whether the values of the two <see cref="JSRuntime"/> objects are
		/// unequal.
		/// </summary>
		/// <param name="left">A <see cref="JSRuntime"/> to compare.</param>
		/// <param name="right">A <see cref="JSRuntime"/> to compare.</param>
		/// <returns>
		/// true if <paramref name="left"/> and <paramref name="right"/> are
		/// unequal; otherwise, false.
		/// </returns>
		public static unsafe bool operator !=(JSRuntime left, JSRuntime right)
		{
			return left._runtime != right._runtime;
		}

	}
}
