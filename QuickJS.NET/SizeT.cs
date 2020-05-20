using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuickJS
{
	[StructLayout(LayoutKind.Sequential)]
	unsafe public struct SizeT
	{
		private void* _value;

		public SizeT(int value)
		{
			_value = (void*)checked((uint)value);
		}

		public SizeT(uint value)
		{
			_value = (void*)value;
		}

		public SizeT(long value)
		{
			checked { _value = (void*)(ulong)value; }
		}

		public SizeT(ulong value)
		{
			_value = checked((void*)value);
		}

		public SizeT(UIntPtr value)
		{
			_value = (void*)value;
		}

		public static implicit operator SizeT(int value)
		{
			return new SizeT(value);
		}

		public static implicit operator SizeT(uint value)
		{
			return new SizeT(value);
		}

		public static implicit operator SizeT(UIntPtr value)
		{
			return new SizeT(value);
		}

		public static implicit operator SizeT(long value)
		{
			return new SizeT(value);
		}

		public static implicit operator SizeT(ulong value)
		{
			return new SizeT(value);
		}

		public static implicit operator int(SizeT value)
		{
			return checked((int)value._value);
		}

		public static implicit operator uint(SizeT value)
		{
			return checked((uint)value._value);
		}

		public static implicit operator UIntPtr(SizeT value)
		{
			return new UIntPtr(value._value);
		}

		public static implicit operator long(SizeT value)
		{
			return checked((long)value._value);
		}

		public static implicit operator ulong(SizeT value)
		{
			return checked((ulong)value._value);
		}

	}
}
