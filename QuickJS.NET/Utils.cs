using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QuickJS
{
	static class Utils
	{
		public unsafe static IntPtr CreateArgv(Encoding encoding, string[] args)
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

		public static void ReleaseArgv(IntPtr argv)
		{
			Marshal.FreeHGlobal(argv);
		}

		public static unsafe byte[] StringToManagedUTF8(string s)
		{
			if (s is null)
				return null;

			Encoding utf8 = Encoding.UTF8;
			fixed (char* s0 = s)
			{
				int count = utf8.GetByteCount(s0, s.Length);
				byte[] buffer = new byte[count + 1];
				fixed (byte* buf = buffer)
				{
					utf8.GetBytes(s0, s.Length, buf, count);
					buf[count] = 0;
				}
				return buffer;
			}
		}

		public static unsafe string PtrToStringUTF8(IntPtr ptr, int length)
		{
#if NETSTANDARD
			return Encoding.UTF8.GetString((byte*)ptr, length);
#else
			var buffer = new byte[length];
			Marshal.Copy(ptr, buffer, 0, length);
			return Encoding.UTF8.GetString(buffer);
#endif
		}

	}
}
