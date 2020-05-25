namespace QuickJS
{
	/// <summary>
	/// Represents the primitive value undefined.
	/// </summary>
	internal struct QuickJSUndefined
	{
		/// <summary>
		/// A read-only field that represents an undefined value.
		/// </summary>
		public static readonly QuickJSUndefined Value = default;

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			return obj is QuickJSUndefined;
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return 0;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return "undefined";
		}

	}
}
