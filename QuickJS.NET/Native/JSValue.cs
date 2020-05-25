using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;
using static QuickJS.Native.QuickJSNativeApi;
using System.Runtime.CompilerServices;

namespace QuickJS.Native
{
	[StructLayout(LayoutKind.Explicit)]
	public struct JSValue
	{
		[StructLayout(LayoutKind.Sequential)]
		internal struct JSTagUnion
		{
			private unsafe void* _padding;
			public JSTag tag;
		}

		static JSValue()
		{
			if (IntPtr.Size == 4)
			{
				JS_FLOAT64_TAG_ADDEND_BITS = (unchecked((ulong)(0x7ff80000 - (int)JSTag.First + 1)) << 32); // quiet NaN encoding
				NaN.uint64 = unchecked(0x7ff8000000000000UL - JS_FLOAT64_TAG_ADDEND_BITS);
			}
			else
			{
				JS_FLOAT64_TAG_ADDEND_BITS = 0UL;
				NaN._tagdata.tag = JSTag.Float64;
				NaN.float64 = double.NaN;
			}
		}

		[FieldOffset(0)]
		internal ulong uint64;
		[FieldOffset(0)]
		internal int int32;
		[FieldOffset(0)]
		internal double float64;
		[FieldOffset(0)]
		internal unsafe void* ptr;
		[FieldOffset(0)]
		internal JSTagUnion _tagdata;

		private static readonly ulong JS_FLOAT64_TAG_ADDEND_BITS;
		public static readonly JSValue NaN;
		public static readonly JSValue Null = JS_MKVAL(JSTag.Null, 0);
		public static readonly JSValue Undefined = JS_MKVAL(JSTag.Undefined, 0);
		public static readonly JSValue False = JS_MKVAL(JSTag.Bool, 0);
		public static readonly JSValue True = JS_MKVAL(JSTag.Bool, 1);
		public static readonly JSValue Exception = JS_MKVAL(JSTag.Exception, 0);
		public static readonly JSValue Uninitialized = JS_MKVAL(JSTag.Uninitialized, 0);

		[MethodImpl(AggressiveInlining)]
		internal static JSValue JS_MKVAL(JSTag tag, int value)
		{
			var v = new JSValue();
			v.uint64 = unchecked((uint)value);
			v._tagdata.tag = tag;
			return v;
		}

		[MethodImpl(AggressiveInlining)]
		internal static JSValue __JS_NewFloat64(JSContext ctx, double d)
		{
			JSValue v = new JSValue();
			v._tagdata.tag = JSTag.Float64;
			v.float64 = d;
			/* normalize NaN */
			if ((v.uint64 & 0x7fffffffffffffff) > 0x7ff0000000000000)
				return JSValue.NaN;
			else
				v.uint64 = v.uint64 - JS_FLOAT64_TAG_ADDEND_BITS;
			return v;
		}

		/// <summary>
		/// Returns a value that indicates whether the specified value is not a
		/// number (NaN).
		/// </summary>
		/// <param name="value">The value to be tested.</param>
		/// <returns>
		/// true if <paramref name="value"/> evaluates to NaN; otherwise, false.
		/// </returns>
		public static bool IsNaN(JSValue value)
		{
			if (IntPtr.Size == 4)
				return (value.uint64 >> 32) == (NaN.uint64 >> 32);
		
			if(value.Tag != JSTag.Float64)
				return false;

			return (value.uint64 & 0x7fffffffffffffff) > 0x7ff0000000000000;
		}

		public int ToInt32()
		{
			return int32;
		}

		public double ToDouble()
		{
			JSValue v = this;
			v.uint64 += JS_FLOAT64_TAG_ADDEND_BITS;
			return v.float64;
		}

		public bool ToBoolean()
		{
			return int32 != 0;
		}

		unsafe public IntPtr ToPointer()
		{
			return new IntPtr(ptr);
		}

		public string GetStringProperty(JSContext context, string name)
		{
			JSValue val = JS_GetPropertyStr(context, this, name);
			try
			{
				return val.ToString(context, true);
			}
			finally
			{
				JS_FreeValue(context, val);
			}
		}

		public int GetInt32Property(JSContext context, string name)
		{
			JSValue val = JS_GetPropertyStr(context, this, name);
			try
			{
				if (val.Tag != JSTag.Int)
					throw new InvalidCastException();
				return val.int32;
			}
			finally
			{
				JS_FreeValue(context, val);
			}
		}

		public string ToString(JSContext ctx, bool cesu8)
		{
			SizeT len;
			string s;
			IntPtr p = JS_ToCStringLen2(ctx, out len, this, cesu8);
			if (p == IntPtr.Zero)
				return null;

			return Utils.PtrToStringUTF8(p, len);
		}

		public string ToString(JSContext ctx)
		{
			return ToString(ctx, true);
		}



		public JSTag Tag
		{
			get
			{
				if ((uint)((_tagdata.tag) - JSTag.First) >= (JSTag.Float64 - JSTag.First))
				{
					return JSTag.Float64;
				}
				return _tagdata.tag;
			}
		}

		public unsafe int RefCount
		{
			get
			{
				if ((uint)_tagdata.tag >= unchecked((uint)JSTag.First))
				{
					JSRefCountHeader* p = (JSRefCountHeader*)ptr;
					return p->ref_count;
				}
				return 0;
			}
		}

		public override string ToString()
		{
			switch (Tag)
			{
				case JSTag.Bool:
					return ToBoolean().ToString();
				case JSTag.Int:
					return ToInt32().ToString();
				case JSTag.Float64:
					return ToDouble().ToString(NumberFormatInfo.InvariantInfo);
				case JSTag.Null:
					return null;
				case JSTag.Undefined:
					return "[undefined]";
				case JSTag.Uninitialized:
					return "[uninitialized]";
				case JSTag.Symbol:
					return "[symbol]";
				case JSTag.String:
					return "[string]";
				case JSTag.Object:
					return "[object]";
			}
			return "[" + Tag.ToString() + "]";
		}
	}


}
