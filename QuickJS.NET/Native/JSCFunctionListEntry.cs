using System;
using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	/// <summary>
	/// C property definition.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct JSCFunctionListEntry
	{
		public IntPtr name;
		private byte prop_flags;
		[MarshalAs(UnmanagedType.U1)]
		public JSDefType def_type;
		public short magic;
		private JSCFunctionListEntry_u u;

		public JSPropertyFlags PropFlags
		{
			get { return (JSPropertyFlags)prop_flags; }
			set { prop_flags = (byte)value; }
		}

		public Func Func
		{
			get { return u.func; }
			set { u.func = value; }
		}

		public GetSet GetSet
		{
			get { return u.getset; }
			set { u.getset = value; }
		}

		public Alias Alias
		{
			get { return u.alias; }
			set { u.alias = value; }
		}

		public PropList PropList
		{
			get { return u.prop_list; }
			set { u.prop_list = value; }
		}

		public IntPtr Str
		{
			get { return u.str; }
			set { u.str = value; }
		}

		public int I32
		{
			get { return u.i32; }
			set { u.i32 = value; }
		}

		public long I64
		{
			get { return u.i64; }
			set { u.i64 = value; }
		}

		public double F64
		{
			get { return u.f64; }
			set { u.f64 = value; }
		}

	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct JSCFunctionListEntry_u
	{
		[FieldOffset(0)]
		public Func func;
		[FieldOffset(0)]
		public GetSet getset;
		[FieldOffset(0)]
		public Alias alias;
		[FieldOffset(0)]
		public PropList prop_list;

		[FieldOffset(0)]
		public IntPtr str;
		[FieldOffset(0)]
		public int i32;
		[FieldOffset(0)]
		public long i64;
		[FieldOffset(0)]
		public double f64;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Func
	{
		public byte length; /* XXX: should move outside union */
		public byte cproto; /* XXX: should move outside union */
		public JSCFunctionType cfunc;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct GetSet
	{
		public JSCFunctionType get;
		public JSCFunctionType set;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Alias
	{
		public IntPtr name;
		public int _base;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PropList
	{
		public IntPtr tab;
		public int len;
	}





}
