using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static QuickJS.Native.QuickJSNativeApi;

namespace QuickJS.Native
{
	/// <summary>
	/// Represents class ID.
	/// </summary>
	/// <remarks>
	/// The type of the C opaque data is determined with the class ID of the object.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct JSClassID
	{
		private int _value;

		/// <summary>
		/// Create a new class ID.
		/// </summary>
		/// <returns>
		/// The new <see cref="JSClassID"/> that this method creates.
		/// </returns>
		public static JSClassID Create()
		{
			var cid = new JSClassID();
			cid = JS_NewClassID(ref cid);
			return cid;
		}

		/// <summary>
		/// Gets the empty class ID.
		/// </summary>
		public static JSClassID Empty
		{
			get { return default; }
		}

		/// <summary>
		/// Gets JS_CLASS_OBJECT
		/// </summary>
		public static JSClassID Object
		{
			get { return new JSClassID { _value = 1 }; }
		}

		/// <summary>
		/// Converts the value of this instance to a 32-bit signed integer.
		/// </summary>
		/// <returns>A 32-bit signed integer equal to the value of this instance.</returns>
		public int ToInt32()
		{
			return _value;
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		/// <summary>
		/// Converts the value of this instance to its equivalent string representation.
		/// </summary>
		/// <returns>The string representation of the value of this instance.</returns>
		public override string ToString()
		{
			return _value.ToString();
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			return (obj is JSClassID a) && a._value == _value;
		}

		/// <summary>
		/// Compares two <see cref="JSClassID"/> structures. The result specifies
		/// whether the values of the two <see cref="JSClassID"/> structures are
		/// equal.
		/// </summary>
		/// <param name="left">A <see cref="JSClassID"/> to compare.</param>
		/// <param name="right">A <see cref="JSClassID"/> to compare.</param>
		/// <returns>
		/// true if <paramref name="left"/> and <paramref name="right"/> are
		/// equal; otherwise, false.
		/// </returns>
		[MethodImpl(AggressiveInlining)]
		public static unsafe bool operator ==(JSClassID left, JSClassID right)
		{
			return left._value == right._value;
		}

		/// <summary>
		/// Compares two <see cref="JSClassID"/> structures. The result specifies
		/// whether the values of the two <see cref="JSClassID"/> structures are
		/// unequal.
		/// </summary>
		/// <param name="left">A <see cref="JSClassID"/> to compare.</param>
		/// <param name="right">A <see cref="JSClassID"/> to compare.</param>
		/// <returns>
		/// true if <paramref name="left"/> and <paramref name="right"/> are
		/// unequal; otherwise, false.
		/// </returns>
		[MethodImpl(AggressiveInlining)]
		public static unsafe bool operator !=(JSClassID left, JSClassID right)
		{
			return left._value != right._value;
		}

	}


}
