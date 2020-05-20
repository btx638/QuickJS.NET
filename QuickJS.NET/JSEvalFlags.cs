using System;
using static QuickJS.Native.QuickJSNativeApi;

namespace QuickJS
{
	/// <summary>
	/// Eval flags.
	/// </summary>
	[Flags]
	public enum JSEvalFlags
	{
		/// <summary>
		/// Global code (default).
		/// </summary>
		Global = (0 << 0),

		/// <summary>
		/// Module code.
		/// </summary>
		Module = (1 << 0),

		/// <summary>
		/// Direct call (internal use).
		/// </summary>
		Direct = (2 << 0),

		/// <summary>
		/// Indirect call (internal use).
		/// </summary>
		Indirect = (3 << 0),

		/// <summary>
		/// JS_EVAL_TYPE_* mask.
		/// </summary>
		TypeMask = (3 << 0),


		/// <summary>
		/// Force &apos;strict&apos; mode.
		/// </summary>
		Strict = (1 << 3),

		/// <summary>
		/// Force &apos;strip&apos; mode.
		/// </summary>
		Strip = (1 << 4),

		/// <summary>
		/// Compile but do not run.
		/// </summary>
		/// <remarks>
		/// The result is an object with a <see cref="JSTag.FunctionBytecode"/>
		/// or <see cref="JSTag.Module"/> tag. It can be executed
		/// <see cref="JS_EvalFunction"/>.
		/// </remarks>
		CompileOnly = (1 << 5),

		/// <summary>
		/// Do not include the stack frames before this eval in the Error()
		/// backtraces.
		/// </summary>
		BacktraceBarrier = (1 << 6),

	}
}
