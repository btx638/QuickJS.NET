using System;
using System.Runtime.InteropServices;
using System.Text;
using QuickJS;
using QuickJS.Native;
using static QuickJS.Native.QuickJSNativeApi;

namespace TestApp
{
	class Program
	{
		unsafe static void Main(string[] args)
		{
			Console.OutputEncoding = Encoding.UTF8;

			using (var rt = new QuickJSRuntime())
			using (var cx = rt.CreateContext())
			{
				cx.StdAddHelpers();
				cx.InitModuleStd("std");
				cx.InitModuleOS("os");

				try
				{
					string utf8path = @"C:\Users\Имя с пробелом\κάτι εκεί\那裡的東西.js";
					cx.Eval(@"throw new Error('текст с пробелом\κάτι εκεί\那裡的東西.123')", utf8path, JSEvalFlags.Global);
				}
				catch (QuickJSException ex)
				{
					Console.WriteLine(ex);
				}

				Console.WriteLine();
				(cx.EvalFile(@"G:\BUILD\QuickJS\repl.js", Encoding.ASCII, JSEvalFlags.Module | JSEvalFlags.Strip) as IDisposable)?.Dispose();
				rt.RunStdLoop(cx);
			}

			Console.WriteLine();
			Console.WriteLine("press key");
			Console.ReadKey();
		}

	}
}
