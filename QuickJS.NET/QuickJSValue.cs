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

		/// <summary>
		/// Creates a new JavaScript object.
		/// </summary>
		/// <param name="context">The context in which to create the new object.</param>
		/// <returns>The new JavaScript object.</returns>
		/// <exception cref="QuickJSException">Cannot create a new object.</exception>
		public static QuickJSValue Create(QuickJSContext context)
		{
			if (context is null)
				throw new ArgumentOutOfRangeException(nameof(context));
			return new QuickJSValue(context, JSValue.CreateObject(context.NativeInstance));
		}

		/// <summary>
		/// Creates a new JavaScript object.
		/// </summary>
		/// <param name="context">The context in which to create the new object.</param>
		/// <param name="classId">The class ID.</param>
		/// <returns>The new JavaScript object.</returns>
		/// <exception cref="QuickJSException">Cannot create a new object.</exception>
		public static QuickJSValue Create(QuickJSContext context, JSClassID classId)
		{
			if (context is null)
				throw new ArgumentOutOfRangeException(nameof(context));
			if (context.Runtime.IsRegisteredClass(classId))
				throw new ArgumentOutOfRangeException(nameof(classId));
			return new QuickJSValue(context, JSValue.CreateObject(context.NativeInstance, classId));
		}

		/// <summary>
		/// Creates a new JavaScript Array object.
		/// </summary>
		/// <param name="context">The context in which to create the new Array object.</param>
		/// <returns>The new JavaScript Array object.</returns>
		/// <exception cref="QuickJSException">Cannot create a new array.</exception>
		public static QuickJSValue CreateArray(QuickJSContext context)
		{
			if (context is null)
				throw new ArgumentOutOfRangeException(nameof(context));
			return new QuickJSValue(context, JSValue.CreateArray(context.NativeInstance));
		}

		/// <summary>
		/// Parses the specified JSON string, constructing the JavaScript value or object described by the string.
		/// </summary>
		/// <param name="context">A pointer to the context in which to create the new object.</param>
		/// <param name="json">The string to parse as JSON.</param>
		/// <param name="filename">The name of the JSON file.</param>
		/// <returns>The <see cref="QuickJSValue"/> corresponding to the given JSON text.</returns>
		public static QuickJSValue FromJSON(QuickJSContext context, string json, string filename)
		{
			if (json == null)
				return null;

			return new QuickJSValue(context, JSValue.FromJSON(context.NativeInstance, json, filename));
		}

		private QuickJSValue(QuickJSContext context, JSValue value)
		{
			if (context is null)
				throw new ArgumentOutOfRangeException(nameof(context));
			if (value.Tag != JSTag.Object)
				throw new ArgumentOutOfRangeException(nameof(value));

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
		/// Gets a value indicating whether this object is an array object. 
		/// </summary>
		public bool IsArray
		{
			get
			{
				return JS_IsArray(_context.NativeInstance, _value) == 1;
			}
		}

		/// <summary>
		/// Gets a <see cref="JSValue"/> referenced by this instance.
		/// </summary>
		public JSValue NativeInstance
		{
			get { return _value; }
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
		/// <param name="argCount">The number of arguments the function expects to receive.</param>
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
		public bool DefineProperty(string name, string value, JSPropertyFlags flags)
		{
			return DefineProperty(name, JSValue.Create(_context.NativeInstance, value), flags);
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

		/// <summary>
		/// Defines a new property directly on an object, or modifies an existing property on an object.
		/// </summary>
		/// <param name="name">The name of the property to be defined or modified.</param>
		/// <param name="getter">
		/// The property getter callback, which the JavaScript engine will call each time
		/// the property&apos;s value is accessed; or null.
		/// </param>
		/// <param name="setter">
		/// The property setter callback, which the JavaScript engine will call each time
		/// the property is assigned; or null.
		/// </param>
		/// <param name="flags">A bitwise combination of the <see cref="JSPropertyFlags"/>.</param>
		/// <returns>true if the property has been defined or redefined; otherwise false.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe bool DefineProperty(string name, JSCFunction getter, JSCFunction setter, JSPropertyFlags flags)
		{
			JSValue getterVal, setterVal;
			getterVal = getter is null ? JSValue.Undefined : _context.CreateFunctionRaw("get_" + name, getter, 0);
			try
			{
				setterVal = setter is null ? JSValue.Undefined : _context.CreateFunctionRaw("set_" + name, setter, 1);
			}
			catch
			{
				JS_FreeValue(_context.NativeInstance, getterVal);
				throw;
			}
			fixed (byte* aName = Utils.StringToManagedUTF8(name))
			{
				return DefinePropertyInternal(aName, getterVal, setterVal, flags);
			}
		}

		/// <summary>
		/// Defines a new property directly on an object, or modifies an existing property on an object.
		/// </summary>
		/// <param name="name">The name of the property to be defined or modified.</param>
		/// <param name="getter">
		/// A <see cref="JSValue"/> containing the property getter callback, which the JavaScript engine
		/// will call each time the property&apos;s value is accessed; or <see cref="JSValue.Undefined"/>.
		/// </param>
		/// <param name="setter">
		/// A <see cref="JSValue"/> containing the property setter callback, which the JavaScript engine
		/// will call each time the property is assigned; or <see cref="JSValue.Undefined"/>.
		/// </param>
		/// <param name="flags">A bitwise combination of the <see cref="JSPropertyFlags"/>.</param>
		/// <returns>true if the property has been defined or redefined; otherwise false.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe bool DefineProperty(string name, JSValue getter, JSValue setter, JSPropertyFlags flags)
		{
			fixed (byte* aName = Utils.StringToManagedUTF8(name))
			{
				return DefinePropertyInternal(aName, getter, setter, flags);
			}
		}

		private unsafe bool DefinePropertyInternal(byte* name, JSValue getter, JSValue setter, JSPropertyFlags flags)
		{
			JSContext context = _context.NativeInstance;

			if (name == null)
			{
				JS_FreeValue(context, getter);
				JS_FreeValue(context, setter);
				throw new ArgumentNullException(nameof(name));
			}

			JSAtom prop = JS_NewAtom(context, name);
			int rv = JS_DefinePropertyGetSet(context, _value, prop, getter, setter, flags & JSPropertyFlags.CWE);
			JS_FreeAtom(context, prop);
			if (rv == -1)
				context.ThrowPendingException();
			return rv == 1;
		}

		private unsafe bool DefinePropertyInternal(byte* name, JSValue value, JSPropertyFlags flags)
		{
			if (name == null)
			{
				JS_FreeValue(_context.NativeInstance, value);
				throw new ArgumentNullException(nameof(name));
			}

			int rv = JS_DefinePropertyValueStr(_context.NativeInstance, _value, name, value, flags & JSPropertyFlags.CWE);
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
		public unsafe bool SetProperty(string name, int value)
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
		public unsafe bool SetProperty(string name, long value)
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
		public unsafe bool SetProperty(string name, double value)
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
		public unsafe bool SetProperty(string name, bool value)
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
		public bool SetProperty(string name, QuickJSValue value)
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
		/// Assigns a <see cref="QuickJSValue"/> value to a property of an object.
		/// </summary>
		/// <param name="name">Name of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		public bool SetProperty(string name, string value)
		{
			return SetProperty(name, JSValue.Create(_context.NativeInstance, value));
		}

		/// <summary>
		/// Assigns a value to a property of an object.
		/// </summary>
		/// <param name="name">Name of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		public unsafe bool SetProperty(string name, JSValue value)
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
		/// Assigns an <see cref="Int32"/> value to a property of an object.
		/// </summary>
		/// <param name="index">The index of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		public bool SetProperty(int index, int value)
		{
			return SetProperty(index, JS_NewInt32(_context.NativeInstance, value));
		}

		/// <summary>
		/// Assigns an <see cref="Int64"/> value to a property of an object.
		/// </summary>
		/// <param name="index">The index of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		public bool SetProperty(int index, long value)
		{
			return SetProperty(index, JS_NewInt64(_context.NativeInstance, value));
		}

		/// <summary>
		/// Assigns a <see cref="Double"/> value to a property of an object.
		/// </summary>
		/// <param name="index">The index of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		public bool SetProperty(int index, double value)
		{
			return SetProperty(index, JS_NewFloat64(_context.NativeInstance, value));
		}

		/// <summary>
		/// Assigns a <see cref="Boolean"/> value to a property of an object.
		/// </summary>
		/// <param name="index">The index of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		public bool SetProperty(int index, bool value)
		{
			return SetProperty(index, JS_NewBool(_context.NativeInstance, value));
		}

		/// <summary>
		/// Assigns a <see cref="QuickJSValue"/> value to a property of an object.
		/// </summary>
		/// <param name="index">The index of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		public bool SetProperty(int index, QuickJSValue value)
		{
			if (value is null)
			{
				return SetProperty(index, JSValue.Null);
			}
			else
			{
				if (!_context.IsCompatibleWith(value._context))
					throw new ArgumentOutOfRangeException(nameof(value));
				return SetProperty(index, value.GetNativeInstance());
			}
		}

		/// <summary>
		/// Assigns a <see cref="QuickJSValue"/> value to a property of an object.
		/// </summary>
		/// <param name="index">The index of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		public bool SetProperty(int index, string value)
		{
			return SetProperty(index, JSValue.Create(_context.NativeInstance, value));
		}

		/// <summary>
		/// Assigns a value to a property of an object.
		/// </summary>
		/// <param name="index">The index of the property to set.</param>
		/// <param name="value">The value to assign to the property.</param>
		/// <returns>On success, returns true; otherwise, false.</returns>
		public bool SetProperty(int index, JSValue value)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException(nameof(index));

			int rv = JS_SetPropertyUint32(_context.NativeInstance, _value, unchecked((uint)index), value);
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
		/// Searches for the property with the specified index.
		/// </summary>
		/// <param name="index">The index of the property.</param>
		/// <returns>
		/// An object representing the property with the specified name, if found;
		/// otherwise, <see cref="Undefined"/>.
		/// </returns>
		public object GetProperty(int index)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException(nameof(index));

			return _context.ConvertJSValueToClrObject(JS_GetPropertyUint32(_context.NativeInstance, _value, unchecked((uint)index)), true);
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
		/// Gets or sets the property with the specified name.
		/// </summary>
		/// <param name="index">The index of the property.</param>
		/// <returns>
		/// An object representing the property with the specified name, if found;
		/// otherwise, <see cref="Undefined"/>.
		/// </returns>
		public object this[int index]
		{
			get
			{
				return GetProperty(index);
			}
			set
			{
				SetProperty(index, _context.ConvertClrObjectToJSValue(value));
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
		/// Converts a JavaScript object or value to a JSON string, optionally replacing values if
		/// a <paramref name="replacer"/> function is specified or optionally including only
		/// the specified properties if a <paramref name="replacer"/> array is specified.
		/// </summary>
		/// <param name="replacer">
		/// A function that alters the behavior of the stringification process, or an array of String
		/// and Number that serve as a whitelist for selecting/filtering the properties of the value
		/// object to be included in the JSON string.<para/>
		/// If this value is null or not provided, all properties of the object are included in the
		/// resulting JSON string.
		/// </param>
		/// <param name="space">
		/// The string (or the first 10 characters of the string, if it&apos;s longer than that)
		/// is used as white space. If this parameter is null, no white space is used.
		/// </param>
		/// <param name="json">
		/// When the method returns, the JSON string representing this <see cref="JSValue"/>.
		/// </param>
		/// <returns>true if the operation is successful; otherwise, false.</returns>
		public bool TryGetJSON(QuickJSValue replacer, string space, out string json)
		{
			if (replacer is null)
				return _value.TryGetJSON(_context.NativeInstance, JSValue.Undefined, space, out json);

			if (!_context.IsCompatibleWith(replacer._context))
				throw new ArgumentOutOfRangeException(nameof(replacer));

			return _value.TryGetJSON(_context.NativeInstance, replacer._value, space, out json);
		}

		/// <summary>
		/// Converts a JavaScript object or value to a JSON string, optionally replacing values if
		/// a <paramref name="replacer"/> function is specified or optionally including only
		/// the specified properties if a <paramref name="replacer"/> array is specified.
		/// </summary>
		/// <param name="replacer">
		/// A function that alters the behavior of the stringification process, or an array of String
		/// and Number that serve as a whitelist for selecting/filtering the properties of the value
		/// object to be included in the JSON string.<para/>
		/// If this value is null or not provided, all properties of the object are included in the
		/// resulting JSON string.
		/// </param>
		/// <param name="space">
		/// Indicates the number of space characters to use as white space;
		/// this number is capped at 10 (if it is greater, the value is just 10).
		/// Values less than 1 indicate that no space should be used.
		/// </param>
		/// <param name="json">
		/// When the method returns, the JSON string representing this <see cref="JSValue"/>.
		/// </param>
		/// <returns>true if the operation is successful; otherwise, false.</returns>
		public bool TryGetJSON(QuickJSValue replacer, int space, out string json)
		{
			if (replacer is null)
				return _value.TryGetJSON(_context.NativeInstance, JSValue.Undefined, space, out json);

			if (!_context.IsCompatibleWith(replacer._context))
				throw new ArgumentOutOfRangeException(nameof(replacer));

			return _value.TryGetJSON(_context.NativeInstance, replacer._value, space, out json);
		}

		/// <summary>
		/// Converts a JavaScript object or value to a JSON string.
		/// </summary>
		/// <param name="json">
		/// When the method returns, the JSON string representing this <see cref="JSValue"/>.
		/// </param>
		/// <returns>true if the operation is successful; otherwise, false.</returns>
		public bool TryGetJSON(out string json)
		{
			return _value.TryGetJSON(_context.NativeInstance, out json);
		}

		/// <summary>
		/// Converts a JavaScript object or value to a JSON string, optionally replacing values if
		/// a <paramref name="replacer"/> function is specified or optionally including only
		/// the specified properties if a <paramref name="replacer"/> array is specified.
		/// </summary>
		/// <param name="replacer">
		/// A function that alters the behavior of the stringification process, or an array of String
		/// and Number that serve as a whitelist for selecting/filtering the properties of the value
		/// object to be included in the JSON string.<para/>
		/// If this value is null or not provided, all properties of the object are included in the
		/// resulting JSON string.
		/// </param>
		/// <param name="space">
		/// The string (or the first 10 characters of the string, if it&apos;s longer than that)
		/// is used as white space. If this parameter is null, no white space is used.
		/// </param>
		/// <returns>The JSON string representing this <see cref="JSValue"/>.</returns>
		public string ToJSON(QuickJSValue replacer, string space)
		{
			if (replacer is null)
				return _value.ToJSON(_context.NativeInstance, JSValue.Undefined, space);

			if (!_context.IsCompatibleWith(replacer._context))
				throw new ArgumentOutOfRangeException(nameof(replacer));

			return _value.ToJSON(_context.NativeInstance, replacer.NativeInstance, space);
		}

		/// <summary>
		/// Converts a JavaScript object or value to a JSON string, optionally replacing values if
		/// a <paramref name="replacer"/> function is specified or optionally including only
		/// the specified properties if a <paramref name="replacer"/> array is specified.
		/// </summary>
		/// <param name="replacer">
		/// A function that alters the behavior of the stringification process, or an array of String
		/// and Number that serve as a whitelist for selecting/filtering the properties of the value
		/// object to be included in the JSON string.<para/>
		/// If this value is null or not provided, all properties of the object are included in the
		/// resulting JSON string.
		/// </param>
		/// <param name="space">
		/// Indicates the number of space characters to use as white space;
		/// this number is capped at 10 (if it is greater, the value is just 10).
		/// Values less than 1 indicate that no space should be used.
		/// </param>
		/// <returns>The JSON string representing this <see cref="JSValue"/>.</returns>
		public string ToJSON(QuickJSValue replacer, int space)
		{
			if (replacer is null)
				return _value.ToJSON(_context.NativeInstance, JSValue.Undefined, space);

			if (!_context.IsCompatibleWith(replacer._context))
				throw new ArgumentOutOfRangeException(nameof(replacer));

			return _value.ToJSON(_context.NativeInstance, replacer.NativeInstance, space);
		}

		/// <summary>
		/// Converts a JavaScript object or value to a JSON string.
		/// </summary>
		/// <returns>The JSON string representing this <see cref="JSValue"/>.</returns>
		public string ToJSON()
		{
			return _value.ToJSON(_context.NativeInstance);
		}

		/// <summary>
		/// Gets an array of all properties found directly in this object.
		/// </summary>
		/// <param name="flags">
		/// A bitwise combination of <see cref="JSGetPropertyNamesFlags"/> values.
		/// </param>
		/// <returns>An array of strings that corresponds to the properties found directly in this object.</returns>
		/// <exception cref="QuickJSException">An exception occurred.</exception>
		public string[] GetOwnPropertyNames(JSGetPropertyNamesFlags flags)
		{
			JSContext ctx = _context.NativeInstance;

			JSPropertyEnum[] props = null;
			try
			{
				if (0 != JS_GetOwnPropertyNames(ctx, out props, _value, flags))
					_context.ThrowPendingException();
				if (props == null)
					return null;
				var names = new string[props.Length];
				for (int i = 0; i < names.Length; i++)
				{
					IntPtr str = JS_AtomToCString(ctx, props[i].atom);
					if (str == IntPtr.Zero)
						continue;
					try
					{
						names[i] = Utils.PtrToStringUTF8(str);
					}
					finally
					{
						JS_FreeCString(ctx, str);
					}
				}
				return names;
			}
			finally
			{
				if (props != null)
				{
					for (int i = 0; i < props.Length; i++)
					{
						JS_FreeAtom(ctx, props[i].atom);
					}
				}
			}
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
