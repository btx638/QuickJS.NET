using System;
using System.Runtime.InteropServices;
using static QuickJS.Native.QuickJSNativeApi;

namespace QuickJS.Native
{
	/// <summary>
	/// Represents a pointer to a native JSContext
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct JSContext
	{
		private unsafe void* _context;

		/// <summary>
		/// A read-only field that represents a null pointer to the native JSContext.
		/// </summary>
		public static readonly JSContext Null = default;

		/// <summary>
		/// Throws the actual exception that is stored in the native JSContext.
		/// </summary>
		/// <remarks>
		/// If there is no pending exception in the context, the method returns
		/// without creating or throwing an exception.
		/// </remarks>
		public unsafe void ThrowPendingException()
		{
			if (_context == null)
				return;

			JSValue exceptionVal = JS_GetException(this);
			if (exceptionVal.Tag == JSTag.Null)
				return;

			try
			{
				if (ErrorInfo.TryCreate(this, exceptionVal, out ErrorInfo errorInfo))
					throw new QuickJSException(errorInfo);

				throw new QuickJSException(exceptionVal.ToString(this));
			}
			finally
			{
				JS_FreeValue(this, exceptionVal);
			}
		}

		/// <inheritdoc/>
		public unsafe override int GetHashCode()
		{
			return new IntPtr(_context).GetHashCode();
		}

		/// <inheritdoc/>
		public unsafe override bool Equals(object obj)
		{
			if (obj is JSContext a)
				return a._context == _context;
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
			return _context;
		}

		/// <summary>
		/// Compares two <see cref="JSContext"/> objects. The result specifies
		/// whether the values of the two <see cref="JSContext"/> objects are
		/// equal.
		/// </summary>
		/// <param name="left">A <see cref="JSContext"/> to compare.</param>
		/// <param name="right">A <see cref="JSContext"/> to compare.</param>
		/// <returns>
		/// true if <paramref name="left"/> and <paramref name="right"/> are
		/// equal; otherwise, false.
		/// </returns>
		public static unsafe bool operator ==(JSContext left, JSContext right)
		{
			return left._context == right._context;
		}

		/// <summary>
		/// Compares two <see cref="JSContext"/> objects. The result specifies
		/// whether the values of the two <see cref="JSContext"/> objects are
		/// unequal.
		/// </summary>
		/// <param name="left">A <see cref="JSContext"/> to compare.</param>
		/// <param name="right">A <see cref="JSContext"/> to compare.</param>
		/// <returns>
		/// true if <paramref name="left"/> and <paramref name="right"/> are
		/// unequal; otherwise, false.
		/// </returns>
		public static unsafe bool operator !=(JSContext left, JSContext right)
		{
			return left._context != right._context;
		}

	}
}
