using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using QuickJS.Native;
using static QuickJS.Native.QuickJSNativeApi;

namespace QuickJS
{
	public class QuickJSValue : IDisposable
	{
		private delegate void CreateVoidDelegate();

		private QuickJSContext _context;
		private readonly JSValue _value;

		public static readonly object Undefined = QuickJSUndefined.Value;

		public QuickJSValue(QuickJSContext context, JSValue value)
		{
			if (context is null)
				throw new ArgumentOutOfRangeException(nameof(context));

			_context = context;
			_value = value;
		}

		/// <summary>
		/// Determines whether a <paramref name="value"/> is undefined or not.
		/// </summary>
		/// <param name="value">The value to be tested.</param>
		/// <returns>true if the given value is undefined; otherwise, false.</returns>
		public static bool IsUndefined(object value)
		{
			return value is QuickJSUndefined;
		}

		/// <summary>
		/// Gets tag of this value.
		/// </summary>
		public JSTag Tag
		{
			get { return _value.Tag; }
		}

		/// <summary>
		/// Creates a native function and assigns it as a property to this JS object.
		/// </summary>
		/// <param name="name">The name of the property to be defined or modified.</param>
		/// <param name="func">The function associated with the property.</param>
		/// <param name="flags">A bitwise combination of the <see cref="JSPropertyFlags"/>.</param>
		/// <returns>true if the property has been defined or redefined; otherwise false.</returns>
		public unsafe bool DefineFunction(string name, JSCFunction func, int argCount, JSPropertyFlags flags)
		{
			fixed (byte* aName = Utils.StringToManagedUTF8(name))
			{
				return DefinePropertyInternal(aName, _context.CreateFunctionRawInternal(aName, func, argCount), flags);
			}
		}

		/// <summary>
		/// Defines a new property directly on an object, or modifies an existing property on an object.
		/// </summary>
		/// <param name="name">The name of the property to be defined or modified.</param>
		/// <param name="value">The value associated with the property.</param>
		/// <param name="flags">A bitwise combination of the <see cref="JSPropertyFlags"/>.</param>
		/// <returns>true if the property has been defined or redefined; otherwise false.</returns>
		public bool DefineProperty(string name, QuickJSValue value, JSPropertyFlags flags)
		{
			if (value._context.NativeInstance != _context.NativeInstance)
				throw new ArgumentOutOfRangeException(nameof(value));
			return DefineProperty(name, JS_DupValue(_context.NativeInstance, value._value), flags);
		}

		/// <summary>
		/// Defines a new property directly on an object, or modifies an existing property on an object.
		/// </summary>
		/// <param name="name">The name of the property to be defined or modified.</param>
		/// <param name="value">The value associated with the property.</param>
		/// <param name="flags">A bitwise combination of the <see cref="JSPropertyFlags"/>.</param>
		/// <returns>true if the property has been defined or redefined; otherwise false.</returns>
		public bool DefineProperty(string name, int value, JSPropertyFlags flags)
		{
			return DefineProperty(name, JS_NewInt32(_context.NativeInstance, value), flags);
		}

		/// <summary>
		/// Defines a new property directly on an object, or modifies an existing property on an object.
		/// </summary>
		/// <param name="name">The name of the property to be defined or modified.</param>
		/// <param name="value">The value associated with the property.</param>
		/// <param name="flags">A bitwise combination of the <see cref="JSPropertyFlags"/>.</param>
		/// <returns>true if the property has been defined or redefined; otherwise false.</returns>
		public bool DefineProperty(string name, long value, JSPropertyFlags flags)
		{
			return DefineProperty(name, JS_NewInt64(_context.NativeInstance, value), flags);
		}

		/// <summary>
		/// Defines a new property directly on an object, or modifies an existing property on an object.
		/// </summary>
		/// <param name="name">The name of the property to be defined or modified.</param>
		/// <param name="value">The value associated with the property.</param>
		/// <param name="flags">A bitwise combination of the <see cref="JSPropertyFlags"/>.</param>
		/// <returns>true if the property has been defined or redefined; otherwise false.</returns>
		public bool DefineProperty(string name, double value, JSPropertyFlags flags)
		{
			return DefineProperty(name, JS_NewFloat64(_context.NativeInstance, value), flags);
		}

		/// <summary>
		/// Defines a new property directly on an object, or modifies an existing property on an object.
		/// </summary>
		/// <param name="name">The name of the property to be defined or modified.</param>
		/// <param name="value">The value associated with the property.</param>
		/// <param name="flags">A bitwise combination of the <see cref="JSPropertyFlags"/>.</param>
		/// <returns>true if the property has been defined or redefined; otherwise false.</returns>
		public bool DefineProperty(string name, bool value, JSPropertyFlags flags)
		{
			return DefineProperty(name, JS_NewBool(_context.NativeInstance, value), flags);
		}

		/// <summary>
		/// Defines a new property directly on an object, or modifies an existing property on an object.
		/// </summary>
		/// <param name="name">The name of the property to be defined or modified.</param>
		/// <param name="value">The value associated with the property.</param>
		/// <param name="flags">A bitwise combination of the <see cref="JSPropertyFlags"/>.</param>
		/// <returns>true if the property has been defined or redefined; otherwise false.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe bool DefineProperty(string name, JSValue value, JSPropertyFlags flags)
		{
			fixed (byte* aName = Utils.StringToManagedUTF8(name))
			{
				return DefinePropertyInternal(aName, value, flags);
			}
		}

		private unsafe bool DefinePropertyInternal(byte* name, JSValue value, JSPropertyFlags flags)
		{
			if (name == null)
			{
				JS_FreeValue(_context.NativeInstance, value);
				throw new ArgumentNullException(nameof(name));
			}

			int rv = JS_DefinePropertyValueStr(_context.NativeInstance, _value, name, value, flags);
			if (rv == -1)
				_context.NativeInstance.ThrowPendingException();
			return rv == 1;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_context is null)
				return;
			_context.Runtime.VerifyAccess();
			JS_FreeValue(_context.NativeInstance, _value);
			_context = null;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			Dispose(true);
		}
	}
}
