using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using QuickJS.Native;

namespace QuickJS
{
	/// <summary>
	/// Contains utilities that the QuickJS.NET uses.
	/// </summary>
	public static class Utils
	{
		internal unsafe static IntPtr CreateArgv(Encoding encoding, string[] args)
		{
			int arraySize = IntPtr.Size * (args.Length + 1);
			int memorySize = arraySize + args.Length;
			foreach (string arg in args)
			{
				memorySize += encoding.GetByteCount(arg);
			}

			byte** argv = (byte**)Marshal.AllocHGlobal(memorySize);
			byte* data = (byte*)argv + arraySize;
			byte* bufferEnd = (byte*)argv + memorySize;

			for (var i = 0; i < args.Length; i++)
			{
				argv[i] = data;
				string arg = args[i];
				fixed (char* arg_ptr = arg)
				{
					data += encoding.GetBytes(arg_ptr, arg.Length, data, (int)(bufferEnd - data));
				}
				data[0] = 0;
				data++;
			}
			argv[args.Length] = null;
			return new IntPtr(argv);
		}

		internal static void ReleaseArgv(IntPtr argv)
		{
			Marshal.FreeHGlobal(argv);
		}

		/// <summary>
		/// Copies the contents of a managed <see cref="string"/> into a byte
		/// array that represents a store for null terminated string.
		/// </summary>
		/// <param name="s">A managed string to be copied.</param>
		/// <returns>
		/// A byte array allocated for the null terminated UTF-8 string, or null
		/// if <paramref name="s"/> is null.
		/// </returns>
		public static byte[] StringToManagedUTF8(string s)
		{
			return StringToManagedUTF8(s, out int _);
		}

		/// <summary>
		/// Copies the contents of a managed <see cref="string"/> into a byte
		/// array that represents a store for null terminated string.
		/// </summary>
		/// <param name="s">A managed string to be copied.</param>
		/// <param name="length">When the method returns, a value containing the length of the UTF-8 string in bytes.</param>
		/// <returns>
		/// A byte array allocated for the null terminated UTF-8 string, or null
		/// if <paramref name="s"/> is null.
		/// </returns>
		public static unsafe byte[] StringToManagedUTF8(string s, out int length)
		{
			if (s is null)
			{
				length = 0;
				return null;
			}

			Encoding utf8 = Encoding.UTF8;
			fixed (char* s0 = s)
			{
				length = utf8.GetByteCount(s0, s.Length);
				byte[] buffer = new byte[length + 1];
				fixed (byte* buf = buffer)
				{
					utf8.GetBytes(s0, s.Length, buf, length);
					buf[length] = 0;
				}
				return buffer;
			}
		}

		/// <summary>
		/// Allocates a managed <see cref="string"/> and copies a specified
		/// number of bytes from an unmanaged UTF8 string into it.
		/// </summary>
		/// <param name="ptr">
		/// The address of the first character of the unmanaged string.
		/// </param>
		/// <param name="length">The number of bytes to copy.</param>
		/// <returns>
		/// A managed string that holds a copy of the unmanaged string if the
		/// value of the ptr parameter is not null; otherwise, this method
		/// returns null.
		/// </returns>
		public static unsafe string PtrToStringUTF8(IntPtr ptr, int length)
		{
			if (ptr == IntPtr.Zero)
				return null;
#if NETSTANDARD
			return Encoding.UTF8.GetString((byte*)ptr, length);
#else
			var buffer = new byte[length];
			Marshal.Copy(ptr, buffer, 0, length);
			return Encoding.UTF8.GetString(buffer);
#endif
		}

		internal unsafe static JSValue ReportException(JSContext cx, Exception ex)
		{
			IntPtr opaque = QuickJSNativeApi.JS_GetContextOpaque(cx);
			if (opaque != IntPtr.Zero)
				((QuickJSContext)GCHandle.FromIntPtr(opaque).Target).SetClrException(ex);

			fixed (byte* msg = Utils.StringToManagedUTF8(ex.Message.Replace("%", "%%")))
			{
				return QuickJSNativeApi.JS_ThrowInternalError(cx, msg, __arglist());
			}
		}

	}
}
