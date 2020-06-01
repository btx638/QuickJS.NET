using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	/// <summary>
	/// Represents a unique value (an atom) identifying a resource in the atom table.
	/// </summary>

	[StructLayout(LayoutKind.Sequential)]
	public struct JSAtom
	{
		private int _value;

		/// <summary>
		/// Converts the value of this instance to a 32-bit signed integer.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer equal to the value of this instance.
		/// </returns>
		public int ToInt32()
		{
			return _value;
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return _value.ToString();
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			if (obj is JSAtom a)
				return a._value == _value;
			return false;
		}
	}

}
