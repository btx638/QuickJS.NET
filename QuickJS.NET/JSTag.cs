namespace QuickJS
{
	public enum JSTag
	{
		/* all tags with a reference count are negative */
		First = -11, /* first negative tag */
		BigDecimal = -11,
		BigInt = -10,
		BigFloat = -9,
		Symbol = -8,
		String = -7,
		Module = -3, /* used internally */
		FunctionBytecode = -2, /* used internally */
		Object = -1,

		Int = 0,
		Bool = 1,
		Null = 2,
		Undefined = 3,
		Uninitialized = 4,
		CatchOffset = 5,
		Exception = 6,
		Float64 = 7,
		/* any larger tag is FLOAT64 if JS_NAN_BOXING */
	};


}
