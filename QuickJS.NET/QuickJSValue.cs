using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using QuickJS.Native;
using static QuickJS.Native.QuickJSNativeApi;

namespace QuickJS
{
	/// <summary>
	/// Wraps a <see cref="JSValue"/> with a reference count.
	/// </summary>
	public class QuickJSValue : IDisposable
	{
		private delegate void CreateVoidDelegate();

		private QuickJSContext _context;
		private readonly JSValue _value;

		/// <summary>
		/// A read-only field that represents the JavaScript undefined value.
		/// </summary>
		public static readonly object Undefined = QuickJSUndefined.Value;

		/// <summary>
		/// Wraps a <see cref="JSValue"/> with a reference count.
		/// </summary>
		/// <param name="context">
		/// The context in which the <paramref name="value"/> was created.
		/// </param>
		/// <param name="value">A <see cref="JSValue"/> with a reference count.</param>
		/// <returns>
		/// An instance of the <see cref="QuickJSValue"/> that is the wrapper
		/// for the specified <paramref name="value"/>.
		/// </returns>
		public static QuickJSValue Wrap(QuickJSContext context, JSValue value)
		{
			return new QuickJSValue(context, value);
		}

		private QuickJSValue(QuickJSContext context, JSValue value)
		{
			if (context is null)
				throw new ArgumentOutOfRangeException(nameof(context));

			_context = context;
			_value = value;
			_context.AddValue(this);
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
		/// Gets a value indicating whether this object is a function object. 
		/// </summary>
		public bool IsFunction
		{
			get
			{
				return JS_IsFunction(_context.NativeInstance, _value);
			}
		}

		/// <summary>
		/// Increments the reference count and returns a <see cref="JSValue"/>.
		/// </summary>
		/// <returns><see cref="JSValue"/> referenced by this instance.</returns>
		public JSValue GetNativeInstance()
		{
			return JS_DupValue(_context.NativeInstance, _value);
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
			if (value is null)
			{
				return DefineProperty(name, JSValue.Null, flags);
			}
			else
			{
				if (!_context.IsCompatibleWith(value._context))
					throw new ArgumentOutOfRangeException(nameof(value));
				return DefineProperty(name, value.GetNativeInstance(), flags);
			}
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

		/// <summary>
		/// Assigns an <see cref="Int32"/> value to a property of an object.
		/// </summary>
		/// <param name="name">Name of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		private unsafe bool SetProperty(string name, int value)
		{
			fixed (byte* aName = Utils.StringToManagedUTF8(name))
			{
				return SetPropertyInternal(aName, JS_NewInt32(_context.NativeInstance, value));
			}
		}

		/// <summary>
		/// Assigns an <see cref="Int64"/> value to a property of an object.
		/// </summary>
		/// <param name="name">Name of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		private unsafe bool SetProperty(string name, long value)
		{
			fixed (byte* aName = Utils.StringToManagedUTF8(name))
			{
				return SetPropertyInternal(aName, JS_NewInt64(_context.NativeInstance, value));
			}
		}

		/// <summary>
		/// Assigns a <see cref="Double"/> value to a property of an object.
		/// </summary>
		/// <param name="name">Name of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		private unsafe bool SetProperty(string name, double value)
		{
			fixed (byte* aName = Utils.StringToManagedUTF8(name))
			{
				return SetPropertyInternal(aName, JS_NewFloat64(_context.NativeInstance, value));
			}
		}

		/// <summary>
		/// Assigns a <see cref="Boolean"/> value to a property of an object.
		/// </summary>
		/// <param name="name">Name of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		private unsafe bool SetProperty(string name, bool value)
		{
			fixed (byte* aName = Utils.StringToManagedUTF8(name))
			{
				return SetPropertyInternal(aName, JS_NewBool(_context.NativeInstance, value));
			}
		}

		/// <summary>
		/// Assigns a <see cref="QuickJSValue"/> value to a property of an object.
		/// </summary>
		/// <param name="name">Name of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		private bool SetProperty(string name, QuickJSValue value)
		{
			if (value is null)
			{
				return SetProperty(name, JSValue.Null);
			}
			else
			{
				if (!_context.IsCompatibleWith(value._context))
					throw new ArgumentOutOfRangeException(nameof(value));
				return SetProperty(name, value.GetNativeInstance());
			}
		}

		/// <summary>
		/// Assigns a value to a property of an object.
		/// </summary>
		/// <param name="name">Name of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		private unsafe bool SetProperty(string name, JSValue value)
		{
			fixed (byte* aName = Utils.StringToManagedUTF8(name))
			{
				return SetPropertyInternal(aName, value);
			}
		}

		private unsafe bool SetPropertyInternal(byte* name, JSValue value)
		{
			if (name == null)
			{
				JS_FreeValue(_context.NativeInstance, value);
				throw new ArgumentNullException(nameof(name));
			}

			int rv = JS_SetPropertyStr(_context.NativeInstance, _value, name, value);
			if (rv == -1)
				_context.NativeInstance.ThrowPendingException();
			return rv == 1;
		}

		/// <summary>
		/// Searches for the property with the specified name.
		/// </summary>
		/// <param name="name">The string containing the name of the property to get.</param>
		/// <returns>
		/// An object representing the property with the specified name, if found;
		/// otherwise, <see cref="Undefined"/>.
		/// </returns>
		public unsafe object GetProperty(string name)
		{
			if (name is null)
				throw new ArgumentNullException(nameof(name));

			fixed (byte* aName = Utils.StringToManagedUTF8(name))
			{
				return _context.ConvertJSValueToClrObject(JS_GetPropertyStr(_context.NativeInstance, _value, aName), true);
			}
		}

		/// <summary>
		/// Gets or sets the property with the specified name.
		/// </summary>
		/// <param name="name">The string containing the name of the property.</param>
		/// <returns>
		/// An object representing the property with the specified name, if found;
		/// otherwise, <see cref="Undefined"/>.
		/// </returns>
		public object this[string name]
		{
			get
			{
				return GetProperty(name);
			}
			set
			{
				SetProperty(name, _context.ConvertClrObjectToJSValue(value));
			}
		}

		/// <summary>
		/// Invokes the function represented by the current instance on the global object.
		/// </summary>
		/// <returns>An object containing the return value of the invoked function.</returns>
		public object Call()
		{
			JSValue value = JS_GetGlobalObject(_context.NativeInstance);
			if (JS_IsException(value))
				_context.NativeInstance.ThrowPendingException();

			try
			{
				return Call(value);
			}
			finally
			{
				JS_FreeValue(_context.NativeInstance, value);
			}
		}

		/// <summary>
		/// Invokes the function represented by the current instance.
		/// </summary>
		/// <param name="thisArg">
		/// The object on which to invoke the function; is &apos;this&apos;
		/// when the function executes.
		/// </param>
		/// <returns>
		/// An object containing the return value of the invoked function.
		/// </returns>
		public object Call(QuickJSValue thisArg)
		{
			return Call(thisArg is null ? JSValue.Null : thisArg._value);
		}

		/// <summary>
		/// Invokes the function represented by the current instance.
		/// </summary>
		/// <param name="thisArg">
		/// The object on which to invoke the function; is &apos;this&apos;
		/// when the function executes.
		/// </param>
		/// <param name="args">
		/// The array of argument values to pass to the function.
		/// </param>
		/// <returns>
		/// An object containing the return value of the invoked function.
		/// </returns>
		public object Call(QuickJSValue thisArg, params JSValue[] args)
		{
			return Call(thisArg is null ? JSValue.Null : thisArg._value, args);
		}

		/// <summary>
		/// Invokes the function represented by the current instance.
		/// </summary>
		/// <param name="thisArg">
		/// The object on which to invoke the function; is &apos;this&apos;
		/// when the function executes.
		/// </param>
		/// <returns>
		/// An object containing the return value of the invoked function.
		/// </returns>
		public unsafe object Call(JSValue thisArg)
		{
			return _context.ConvertJSValueToClrObject(JS_Call(_context.NativeInstance, _value, thisArg, 0, default(JSValue*)), true);
		}

		/// <summary>
		/// Invokes the function represented by the current instance.
		/// </summary>
		/// <param name="thisArg">
		/// The object on which to invoke the function; is &apos;this&apos;
		/// when the function executes.
		/// </param>
		/// <param name="args">
		/// The array of argument values to pass to the function.
		/// </param>
		/// <returns>
		/// An object containing the return value of the invoked function.
		/// </returns>
		public object Call(JSValue thisArg, params JSValue[] args)
		{
			if (args is null)
				return Call(thisArg);

			return _context.ConvertJSValueToClrObject(JS_Call(_context.NativeInstance, _value, thisArg, args.Length, args), true);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing,
		/// or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">
		/// Indicates whether the method call comes from a <see cref="Dispose()"/> method
		/// (true) or from a finalizer (false).
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (_context is null)
				return;
			_context.Runtime.VerifyAccess();
			_context.RemoveValue(this);
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
