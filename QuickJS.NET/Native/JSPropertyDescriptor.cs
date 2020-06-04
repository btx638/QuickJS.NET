using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static QuickJS.Native.QuickJSNativeApi;

namespace QuickJS.Native
{
	/// <summary>
	/// A descriptor is used to declare whether an attribute can be written to whether
	/// it can delete an object that can enumerate and specify content.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct JSPropertyDescriptor
	{
		[MarshalAs(UnmanagedType.I4)]
		public JSPropertyFlags flags;
		public JSValue value;
		public JSValue getter;
		public JSValue setter;
	}

	/// <summary>
	/// Represents a pointer to a <see cref="JSPropertyDescriptor"/>.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct JSPropertyDescriptorHandle
	{
		private readonly JSPropertyDescriptor* _handle;

		/// <summary>
		/// A read-only field that represents a handle that has been initialized to zero.
		/// </summary>
		public static readonly JSPropertyDescriptorHandle Zero = default;

		private JSPropertyDescriptorHandle(IntPtr handle)
		{
			_handle = (JSPropertyDescriptor*)handle;
		}

		/// <summary>
		/// Gets a value indicating whether the handle is allocated.
		/// </summary>
		public bool IsAllocated
		{
			get { return _handle != default; }
		}

		/// <summary>
		/// A bitwise combination of <see cref="JSPropertyFlags"/> that
		/// declares that a property can be modified, deleted, enumerated,
		/// and rewritten.
		/// </summary>
		public JSPropertyFlags Flags
		{
			get
			{
				if (!IsAllocated)
					throw new InvalidOperationException();
				return _handle->flags;
			}
			set
			{
				if (!IsAllocated)
					throw new InvalidOperationException();
				_handle->flags = value;
			}
		}

		/// <summary>
		/// Describes the value of the specified property, which can be any valid
		/// JavaScript value (function, object, string...).
		/// </summary>
		public JSValue Value
		{
			get
			{
				if (!IsAllocated)
					throw new InvalidOperationException();
				return _handle->value;
			}
			set
			{
				if (!IsAllocated)
					throw new InvalidOperationException();
				_handle->value = value;
			}
		}

		/// <summary>
		/// The &apos;get&apos; syntax binds an object property to a function
		/// that will be called when that property is looked up.
		/// </summary>
		public JSValue Getter
		{
			get
			{
				if (!IsAllocated)
					throw new InvalidOperationException();
				return _handle->value;
			}
			set
			{
				if (!IsAllocated)
					throw new InvalidOperationException();
				_handle->getter = value;
			}
		}

		/// <summary>
		/// The &apos;set&apos; syntax binds an object property to a function
		/// to be called when there is an attempt to set that property.
		/// </summary>
		public JSValue Setter
		{
			get
			{
				if (!IsAllocated)
					throw new InvalidOperationException();
				return _handle->setter;
			}
			set
			{
				if (!IsAllocated)
					throw new InvalidOperationException();
				_handle->setter = value;
			}
		}

		/// <summary>
		/// Allocates a handle for the new <see cref="JSPropertyDescriptor"/>.
		/// </summary>
		public static JSPropertyDescriptorHandle Allocate()
		{
			return new JSPropertyDescriptorHandle(Marshal.AllocHGlobal(sizeof(JSPropertyDescriptor)));
		}

		/// <summary>
		/// Releases a <see cref="JSPropertyDescriptorHandle"/>.
		/// </summary>
		public void Free()
		{
			Marshal.FreeHGlobal(new IntPtr(_handle));
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return new IntPtr(_handle).GetHashCode();
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return new IntPtr(_handle).ToString();
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			if (obj is JSPropertyDescriptorHandle a)
				return a._handle == _handle;
			return false;
		}

		/// <summary>
		/// Returns a value indicating whether two <see cref="JSPropertyDescriptorHandle"/> objects are equal.
		/// </summary>
		/// <param name="left">A <see cref="JSPropertyDescriptorHandle"/> to compare.</param>
		/// <param name="right">A <see cref="JSPropertyDescriptorHandle"/> to compare.</param>
		/// <returns>
		/// true if <paramref name="left"/> and <paramref name="right"/> are
		/// equal; otherwise, false.
		/// </returns>
		[MethodImpl(AggressiveInlining)]
		public static unsafe bool operator ==(JSPropertyDescriptorHandle left, JSPropertyDescriptorHandle right)
		{
			return left._handle == right._handle;
		}

		/// <summary>
		/// Returns a value indicating whether two <see cref="JSPropertyDescriptorHandle"/> objects are not equal.
		/// </summary>
		/// <param name="left">A <see cref="JSPropertyDescriptorHandle"/> to compare.</param>
		/// <param name="right">A <see cref="JSPropertyDescriptorHandle"/> to compare.</param>
		/// <returns>
		/// true if <paramref name="left"/> and <paramref name="right"/> are
		/// unequal; otherwise, false.
		/// </returns>
		[MethodImpl(AggressiveInlining)]
		public static unsafe bool operator !=(JSPropertyDescriptorHandle left, JSPropertyDescriptorHandle right)
		{
			return left._handle != right._handle;
		}

	}


}
