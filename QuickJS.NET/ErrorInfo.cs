using System;
using System.Collections.Generic;
using System.Text;
using QuickJS.Native;
using static QuickJS.Native.QuickJSNativeApi;

namespace QuickJS
{
	public struct ErrorInfo
	{
		public static bool TryCreate(JSContext context, JSValue exception, out ErrorInfo errorInfo)
		{
			errorInfo = new ErrorInfo();

			if (!JS_IsError(context, exception))
				return false;

			errorInfo.Name = exception.GetStringProperty(context, "name");
			errorInfo.Message = exception.GetStringProperty(context, "message");
			errorInfo.Stack = exception.GetStringProperty(context, "stack");
			return true;
		}

		/// <summary>
		/// Gets the error name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the error message.
		/// </summary>
		public string Message { get; private set; }

		/// <summary>
		/// Get the stack trace.
		/// </summary>
		public string Stack { get; private set; }

	}
}
