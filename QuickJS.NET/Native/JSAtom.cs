using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static QuickJS.Native.QuickJSNativeApi;

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

		/// <summary>
		/// Converts a <see cref="JSAtom"/> to a managed string.
		/// </summary>
		/// <param name="ctx">
		/// A pointer to a JS context from which to derive runtime information.
		/// </param>
		/// <returns>
		/// On success, returns a string representation of the resource that
		/// this <see cref="JSAtom"/> points to; otherwise it returns null.
		/// </returns>
		public string ToString(JSContext ctx)
		{
			IntPtr str = JS_AtomToCString(ctx, this);
			try
			{
				return Utils.PtrToStringUTF8(str);
			}
			finally
			{
				JS_FreeCString(ctx, str);
			}
		}

		/// <summary>
		/// Converts a <see cref="JSAtom"/> to a JS string.
		/// </summary>
		/// <param name="ctx">
		/// A pointer to a JS context from which to derive runtime information.
		/// </param>
		/// <returns>
		/// On success, returns a <see cref="JSValue"/> containing the string
		/// representation of a resource that this <see cref="JSAtom"/>
		/// points to; otherwise it returns <see cref="JSValue.Exception"/>.
		/// </returns>
		[MethodImpl(AggressiveInlining)]
		public JSValue ToStringValue(JSContext ctx)
		{
			return JS_AtomToString(ctx, this);
		}

		/// <summary>
		/// Converts a <see cref="JSAtom"/> to a <see cref="JSValue"/>.
		/// </summary>
		/// <param name="ctx">
		/// A pointer to a JS context from which to derive runtime information.
		/// </param>
		/// <returns>
		/// On success, returns a <see cref="JSValue"/> containing a resource
		/// that this <see cref="JSAtom"/> points to; otherwise it returns
		/// <see cref="JSValue.Exception"/>.
		/// </returns>
		[MethodImpl(AggressiveInlining)]
		public JSValue ToValue(JSContext ctx)
		{
			return JS_AtomToString(ctx, this);
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

		/// <summary>
		/// Compares two <see cref="JSAtom"/> objects. The result specifies
		/// whether the values of the two <see cref="JSAtom"/> objects are
		/// equal.
		/// </summary>
		/// <param name="left">A <see cref="JSAtom"/> to compare.</param>
		/// <param name="right">A <see cref="JSAtom"/> to compare.</param>
		/// <returns>
		/// true if <paramref name="left"/> and <paramref name="right"/> are
		/// equal; otherwise, false.
		/// </returns>
		public static bool operator ==(JSAtom left, JSAtom right)
		{
			return left._value == right._value;
		}

		/// <summary>
		/// Compares two <see cref="JSAtom"/> objects. The result specifies
		/// whether the values of the two <see cref="JSAtom"/> objects are
		/// unequal.
		/// </summary>
		/// <param name="left">A <see cref="JSAtom"/> to compare.</param>
		/// <param name="right">A <see cref="JSAtom"/> to compare.</param>
		/// <returns>
		/// true if <paramref name="left"/> and <paramref name="right"/> are
		/// unequal; otherwise, false.
		/// </returns>
		public static bool operator !=(JSAtom left, JSAtom right)
		{
			return left._value != right._value;
		}

	}

}
