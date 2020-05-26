using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;
using static QuickJS.Native.QuickJSNativeApi;
using System.Runtime.CompilerServices;

namespace QuickJS.Native
{
	/// <summary>
	/// Holds a JavaScript value and provides type-testing and conversion functions.
	/// </summary>
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

		/// <summary>
		/// Represents a value that is not a number.
		/// </summary>
		public static readonly JSValue NaN;

		/// <summary>
		/// Represents the intentional absence of any object value.
		/// It is one of JavaScript&apos;s primitive values and
		/// is treated as falsy for boolean operations.
		/// </summary>
		public static readonly JSValue Null = JS_MKVAL(JSTag.Null, 0);

		/// <summary>
		/// Represents the primitive value undefined.
		/// </summary>
		public static readonly JSValue Undefined = JS_MKVAL(JSTag.Undefined, 0);

		/// <summary>
		/// Represents a JavaScript false value.
		/// </summary>
		public static readonly JSValue False = JS_MKVAL(JSTag.Bool, 0);

		/// <summary>
		/// Represents a JavaScript true value.
		/// </summary>
		public static readonly JSValue True = JS_MKVAL(JSTag.Bool, 1);

		/// <summary>
		/// Represents an exception value.
		/// </summary>
		public static readonly JSValue Exception = JS_MKVAL(JSTag.Exception, 0);

		/// <summary>
		/// Represents an unitialized value. 
		/// </summary>
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

		/// <summary>
		/// Returns a <see cref="Int32"/> for the <see cref="JSValue"/>.
		/// </summary>
		/// <returns>A signed integer value.</returns>
		/// <exception cref="InvalidCastException">
		/// The <see cref="JSValue"/> is not a signed integer value.
		/// </exception>
		public int ToInt32()
		{
			if (TryGetInt32(out int value))
				return value;
			throw new InvalidCastException();
		}

		/// <summary>
		/// Tries to get the <see cref="Int32"/> value of <see cref="JSValue"/>.
		/// </summary>
		/// <param name="value">A signed integer value.</param>
		/// <returns>true if the operation is successful; otherwise, false.</returns>
		public bool TryGetInt32(out int value)
		{
			if (Tag == JSTag.Int)
			{
				value = this.int32;
				return true;
			}
			value = 0;
			return false;
		}

		/// <summary>
		/// Returns a <see cref="Double"/> for the <see cref="JSValue"/>.
		/// </summary>
		/// <returns>A floating-point value.</returns>
		/// <exception cref="InvalidCastException">
		/// The <see cref="JSValue"/> is not a floating-point value.
		/// </exception>
		public double ToDouble()
		{
			if (TryGetDouble(out double value))
				return value;
			throw new InvalidCastException();
		}

		/// <summary>
		/// Tries to get the <see cref="Double"/> value of <see cref="JSValue"/>.
		/// </summary>
		/// <param name="value">A floating-point value.</param>
		/// <returns>true if the operation is successful; otherwise, false.</returns>
		public bool TryGetDouble(out double value)
		{
			if (Tag == JSTag.Float64)
			{
				JSValue v = this;
				v.uint64 += JS_FLOAT64_TAG_ADDEND_BITS;
				value = v.float64;
				return true;
			}
			value = 0;
			return false;
		}

		/// <summary>
		/// Converts the JavaScript value to a native floating-point value.
		/// </summary>
		/// <returns>A floating-point value.</returns>
		/// <exception cref="InvalidCastException">
		/// The <see cref="JSValue"/> is not a number value.
		/// </exception>
		public double ToNumber()
		{
			if (TryGetAsNumber(out double value))
				return value;
			throw new InvalidCastException();
		}

		/// <summary>
		/// Tries to convert the JavaScript value to a native floating-point value.
		/// </summary>
		/// <param name="value">The result of the conversion operation.</param>
		/// <returns>true if the operation is successful; otherwise, false.</returns>
		public bool TryGetAsNumber(out double value)
		{
			switch (Tag)
			{
				case JSTag.Int:
					value = int32;
					return true;
				case JSTag.Float64:
					JSValue v = this;
					v.uint64 += JS_FLOAT64_TAG_ADDEND_BITS;
					value = v.float64;
					return true;
				default:
					value = 0;
					return false;
			}
		}

		/// <summary>
		/// Returns a <see cref="Boolean"/> for the <see cref="JSValue"/>.
		/// </summary>
		/// <returns>A <see cref="Boolean"/> value.</returns>
		/// <exception cref="InvalidCastException">
		/// The <see cref="JSValue"/> is not a <see cref="Boolean"/> value.
		/// </exception>
		public bool ToBoolean()
		{
			if (Tag == JSTag.Bool)
				return int32 != 0;
			throw new InvalidCastException();
		}

		/// <summary>
		/// Tries to get the <see cref="Boolean"/> value of <see cref="JSValue"/>.
		/// </summary>
		/// <param name="value">A <see cref="Boolean"/> value.</param>
		/// <returns>true if the operation is successful; otherwise, false.</returns>
		public bool TryGetBoolean(out bool value)
		{
			if (Tag == JSTag.Bool)
			{
				value = (int32 != 0);
				return true;
			}
			value = false;
			return false;
		}

		/// <summary>
		/// Converts the JavaScript value to a <see cref="Boolean"/> value.
		/// </summary>
		/// <returns>A <see cref="Boolean"/> value.</returns>
		/// <exception cref="InvalidCastException">
		/// The value of the <see cref="JSValue"/> cannot be converted to a <see cref="Boolean"/> value.
		/// </exception>
		public bool ConvertToBoolean()
		{
			if (TryConvertToBoolean(out bool value))
				return value;
			throw new InvalidCastException();
		}

		/// <summary>
		/// Tries to convert the JavaScript value to a <see cref="Boolean"/> value.
		/// </summary>
		/// <param name="value">A <see cref="Boolean"/> value.</param>
		/// <returns>true if the operation is successful; otherwise, false.</returns>
		public bool TryConvertToBoolean(out bool value)
		{
			value = (int32 != 0);
			JSTag tag = this.Tag;
			return tag < JSTag.Uninitialized || tag > JSTag.Exception;
		}

		/// <summary>
		/// Gets a pointer to a native JSRefCountHeader.
		/// </summary>
		/// <returns>A pointer to memory containing a native JSRefCountHeader.</returns>
		public unsafe IntPtr ToPointer()
		{
			if (Tag <= JSTag.Object)
				return new IntPtr(ptr);
			throw new InvalidCastException();
		}

		/// <summary>
		/// Returns the value of the specified JavaScript property
		/// in this  <see cref="JSValue"/>.
		/// </summary>
		/// <param name="context">
		/// The context that <see cref="JSValue"/> belongs to.
		/// </param>
		/// <param name="name">The name of the property.</param>
		/// <returns>
		/// A string containing the value of the specified
		/// JavaScript property.
		/// </returns>
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

		/// <summary>
		/// Returns the value of the specified JavaScript property in this
		/// <see cref="JSValue"/> as <see cref="Int32"/> value.
		/// </summary>
		/// <param name="context">
		/// The context that <see cref="JSValue"/> belongs to.
		/// </param>
		/// <param name="name">The name of the property.</param>
		/// <returns>
		/// An <see cref="Int32"/> value that represents the value of
		/// the specified JavaScript property.
		/// </returns>
		/// <exception cref="InvalidCastException">
		/// The value of the specified JavaScript property is not
		/// an <see cref="Int32"/> value.
		/// </exception>
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

		/// <summary>
		/// Returns a string representation of the value of the current instance.
		/// </summary>
		/// <param name="ctx">The context that <see cref="JSValue"/> belongs to.</param>
		/// <param name="cesu8"></param>
		/// <returns>A string representation of the value of the current instance.</returns>
		public string ToString(JSContext ctx, bool cesu8)
		{
			SizeT len;
			string s;
			IntPtr p = JS_ToCStringLen2(ctx, out len, this, cesu8);
			if (p == IntPtr.Zero)
				return null;

			return Utils.PtrToStringUTF8(p, len);
		}

		/// <summary>
		/// Returns a string representation of the value of the current instance.
		/// </summary>
		/// <param name="ctx">The context that <see cref="JSValue"/> belongs to.</param>
		/// <returns>A string representation of the value of the current instance.</returns>
		public string ToString(JSContext ctx)
		{
			return ToString(ctx, true);
		}

		/// <summary>
		/// Gets the tag of <see cref="JSValue"/>. 
		/// </summary>
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

		/// <summary>
		/// Gets the value of the reference count.
		/// </summary>
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

		/// <summary>
		/// Returns a context-independed string representation of the value of
		/// the current instance or its type name.
		/// </summary>
		/// <returns>
		/// A string representation of the current instance.
		/// </returns>
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
				case JSTag.First:
					return "[BigDecimal]";
			}
			return "[" + Tag.ToString() + "]";
		}

		/// <summary>
		/// Converts the specified <see cref="JSValue"/> to a 64-bit unsigned integer.
		/// </summary>
		/// <param name="value">The <see cref="JSValue"/> to convert.</param>
		/// <exception cref="InvalidCastException">On a 64-bit platform.</exception>
		/// <remarks>This should only be used on 32 bit platforms to convert return values.</remarks>
		public static unsafe implicit operator ulong(JSValue value)
		{
			if (sizeof(JSValue) != sizeof(ulong))
				throw new InvalidCastException();
			return value.uint64;
		}

	}


}
