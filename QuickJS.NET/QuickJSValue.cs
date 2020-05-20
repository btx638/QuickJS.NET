using System;
using System.Collections.Generic;
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

		public JSTag Tag
		{
			get { return _value.Tag; }
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_context is null)
				return;
			_context.Runtime.VerifyAccess();
			JS_FreeValue(_context.NativeInstance, _value);
			_context = null;
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
