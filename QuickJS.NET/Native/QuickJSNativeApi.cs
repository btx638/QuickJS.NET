using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Security;

namespace QuickJS.Native
{
	/// <summary>
	/// Global QuickJS methods are exposed through this class.
	/// </summary>
	public static class QuickJSNativeApi
	{
		internal const MethodImplOptions AggressiveInlining = (MethodImplOptions)256;//.AggressiveInlining;

		#region quickjs-libc.h

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSModuleDef js_init_module_std(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string module_name);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSModuleDef js_init_module_os(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string module_name);


		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void js_std_add_helpers(JSContext ctx, int argc, IntPtr argv);
		//void js_std_add_helpers(JSContext* ctx, int argc, char** argv);

		/// <summary>
		/// Runs main loop which calls the user JS callbacks.
		/// </summary>
		/// <param name="ctx">The JavaScript context.</param>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void js_std_loop(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void js_std_free_handlers(JSRuntime rt);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void js_std_dump_error(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern IntPtr js_load_file(JSContext ctx, out SizeT bufferSize, [MarshalAs(UnmanagedType.LPStr)] string filename);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int js_module_set_import_meta(JSContext ctx, [In] JSValue func_val, [MarshalAs(UnmanagedType.Bool)] bool use_realpath, [MarshalAs(UnmanagedType.Bool)] bool is_main);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSModuleDef js_module_loader(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string module_name, IntPtr opaque);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern void js_std_eval_binary(JSContext ctx, byte* buffer, SizeT bufferSize, JSEvalFlags flags);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void js_std_eval_binary(JSContext ctx, IntPtr buffer, SizeT bufferSize, JSEvalFlags flags);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void js_std_eval_binary(JSContext ctx, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] buffer, SizeT bufferSize, JSEvalFlags flags);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern void js_std_promise_rejection_tracker(JSContext ctx, [In] JSValue promise, [In] JSValue reason, [MarshalAs(UnmanagedType.Bool)] bool is_handled, void* opaque);

		#endregion

		/// <summary>
		/// Initializes a new JavaScript runtime environment.
		/// </summary>
		/// <returns>
		/// On success, returns a pointer the newly created runtime, which the
		/// caller must later destroy using <see cref="JS_FreeRuntime"/>.
		/// Otherwise, returns null.
		/// </returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSRuntime JS_NewRuntime();

		/// <summary>
		/// Sets a string containing information about the specified runtime.
		/// </summary>
		/// <param name="rt">The JavaScript runtime.</param>
		/// <param name="info">An information about the specified runtime.</param>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern void JS_SetRuntimeInfo(JSRuntime rt, [MarshalAs(UnmanagedType.LPStr)] string info);

		/// <summary>
		/// Sets the global memory allocation limit to a given JSRuntime.
		/// </summary>
		/// <param name="rt">The JavaScript runtime.</param>
		/// <param name="limit">The global memory allocation limit.</param>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_SetMemoryLimit(JSRuntime rt, SizeT limit);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_SetGCThreshold(JSRuntime rt, SizeT gc_threshold);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_SetMaxStackSize(JSRuntime rt, SizeT stack_size);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSRuntime JS_NewRuntime2(in JSMallocFunctions mf, IntPtr opaque);
		//public static extern JSRuntime JS_NewRuntime2(const JSMallocFunctions* mf, void* opaque);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_FreeRuntime(JSRuntime rt);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr JS_GetRuntimeOpaque(JSRuntime rt);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_SetRuntimeOpaque(JSRuntime rt, IntPtr opaque);

		/// <summary>
		/// Marks the object <paramref name="val"/> that the cycle removal algorithm can find it.
		/// </summary>
		/// <param name="rt">The JavaScript runtime.</param>
		/// <param name="val">The JavaScript object.</param>
		/// <param name="mark_func">A method that QuickJS uses to mark objects.</param>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_MarkValue(JSRuntime rt, [In] JSValue val, JS_MarkFunc mark_func);

		/// <summary>
		/// Forces an immediate garbage collection.
		/// </summary>
		/// <param name="rt">A pointer to the runtime to for which to perform garbage collection.</param>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_RunGC(JSRuntime rt);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool JS_IsLiveObject(JSRuntime rt, [In] JSValue obj);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSContext JS_NewContext(JSRuntime rt);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_FreeContext(JSContext s);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSContext JS_DupContext(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr JS_GetContextOpaque(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_SetContextOpaque(JSContext ctx, IntPtr opaque);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSRuntime JS_GetRuntime(JSContext ctx);

		/// <summary>
		/// Sets the class prototype object.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="class_id">The class identifier.</param>
		/// <param name="obj">The class prototype object.</param>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_SetClassProto(JSContext ctx, JSClassID class_id, JSValue obj);

		/// <summary>
		/// Returns the builtin class prototype object.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="class_id">The class identifier.</param>
		/// <returns>The class prototype object.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_GetClassProto(JSContext ctx, JSClassID class_id);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSContext JS_NewContextRaw(JSRuntime runtime);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicBaseObjects(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicDate(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicEval(JSContext context);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicStringNormalize(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicRegExpCompiler(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicRegExp(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicJSON(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicProxy(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicMapSet(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicTypedArrays(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicPromise(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicBigInt(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicBigFloat(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicBigDecimal(JSContext ctx);

		/// <summary>
		/// Enable operator overloading. Must be called after all overloadable base types are initialized.
		/// </summary>
		/// <param name="ctx">The pointer the native JSContext.</param>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_AddIntrinsicOperators(JSContext ctx);

		/* enable "use math" */
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_EnableBignumExt(JSContext ctx, [MarshalAs(UnmanagedType.Bool)] bool enable);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern JSValue js_string_codePointRange(JSContext ctx, [In] JSValue this_val, int argc, JSValue* argv);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr js_malloc_rt(JSRuntime rt, SizeT size);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void js_free_rt(JSRuntime rt, IntPtr ptr);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr js_realloc_rt(JSRuntime rt, IntPtr ptr, SizeT size);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern SizeT js_malloc_usable_size_rt(JSRuntime rt, [In] IntPtr ptr);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr js_mallocz_rt(JSRuntime rt, SizeT size);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr js_malloc(JSContext ctx, SizeT size);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void js_free(JSContext ctx, IntPtr ptr);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr js_realloc(JSContext ctx, IntPtr ptr, SizeT size);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern SizeT js_malloc_usable_size(JSContext ctx, [In] IntPtr ptr);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr js_realloc2(JSContext ctx, IntPtr ptr, SizeT size, out SizeT pslack);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr js_mallocz(JSContext ctx, SizeT size);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr js_strdup(JSContext ctx, [In] IntPtr str);
		//char *js_strdup(JSContext *ctx, const char *str);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr js_strndup(JSContext ctx, [In] IntPtr s, SizeT n);
		//char *js_strndup(JSContext *ctx, const char *s, size_t n);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_ComputeMemoryUsage(JSRuntime rt, out JSMemoryUsage s);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_DumpMemoryUsage(IntPtr file, in JSMemoryUsage s, JSRuntime rt);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSAtom JS_NewAtomLen(JSContext ctx, [In, MarshalAs(UnmanagedType.LPStr)] string str, SizeT len);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSAtom JS_NewAtomLen(JSContext ctx, byte* str, SizeT len);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSAtom JS_NewAtom(JSContext ctx, [In, MarshalAs(UnmanagedType.LPStr)] string str);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSAtom JS_NewAtom(JSContext ctx, byte* str);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSAtom JS_NewAtomUInt32(JSContext ctx, uint n);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSAtom JS_DupAtom(JSContext ctx, JSAtom v);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_FreeAtom(JSContext ctx, JSAtom v);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_FreeAtomRT(JSRuntime rt, JSAtom v);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_AtomToValue(JSContext ctx, JSAtom atom);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_AtomToString(JSContext ctx, JSAtom atom);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr JS_AtomToCString(JSContext ctx, JSAtom atom);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSAtom JS_ValueToAtom(JSContext ctx, [In] JSValue val);

		/// <summary>
		/// Returns an assigned class ID. A new class ID is allocated if a value
		/// of the <paramref name="class_id"/> is empty.
		/// </summary>
		/// <param name="class_id">
		/// The preferred class ID. The <paramref name="class_id"/> will be modified
		/// if its value is empty.
		/// </param>
		/// <returns>The class ID.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSClassID JS_NewClassID(ref JSClassID class_id);

		/// <summary>
		/// Create a new object internal class. 
		/// </summary>
		/// <param name="rt">The JavaScript runtime.</param>
		/// <param name="class_id">The class ID.</param>
		/// <param name="class_def">A class definition.</param>
		/// <returns>Return -1 if error, 0 if OK.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_NewClass(JSRuntime rt, JSClassID class_id, in JSClassDef class_def);


		//[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		//public static extern int JS_NewClass(JSRuntime rt, JSClassID class_id, [MarshalAs(UnmanagedType.LPStruct)] JSClassDef class_def);

		/// <summary>
		/// Determines whether a class with the specified ID is available in the given JavaScript runtime.
		/// </summary>
		/// <param name="rt">The JavaScript runtime.</param>
		/// <param name="class_id">The class ID.</param>
		/// <returns>true if a class with the specified ID is registered; otherwise, false.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool JS_IsRegisteredClass(JSRuntime rt, JSClassID class_id);

		/// <summary>
		/// Creates a value holding a boolean value.
		/// </summary>
		/// <param name="ctx">A pointer to the native JSContext in which the value will be created.</param>
		/// <param name="value">A boolean value.</param>
		/// <returns>A <see cref="JSValue"/> holding a boolean value.</returns>
		[MethodImpl(AggressiveInlining)]
		public static JSValue JS_NewBool(JSContext ctx, bool value)
		{
			return JSValue.JS_MKVAL(JSTag.Bool, value ? 1 : 0);
		}

		/// <summary>
		/// Creates a <see cref="JSValue"/> holding a 32-bit signed integer.
		/// </summary>
		/// <param name="ctx">A pointer to the native JSContext in which the value will be created.</param>
		/// <param name="value">A 32-bit signed integer.</param>
		/// <returns>A <see cref="JSValue"/> holding a 32-bit signed integer.</returns>
		[MethodImpl(AggressiveInlining)]
		public static JSValue JS_NewInt32(JSContext ctx, int value)
		{
			return JSValue.JS_MKVAL(JSTag.Int, value);
		}

		[MethodImpl(AggressiveInlining)]
		public static JSValue JS_NewCatchOffset(JSContext ctx, int value)
		{
			return JSValue.JS_MKVAL(JSTag.CatchOffset, value);
		}

		/// <summary>
		/// Creates a <see cref="JSValue"/> holding a 64-bit signed integer.
		/// </summary>
		/// <param name="ctx">A pointer to the native JSContext in which the value will be created.</param>
		/// <param name="val">A 64-bit signed integer.</param>
		/// <returns>A <see cref="JSValue"/> holding a 64-bit signed integer.</returns>
		[MethodImpl(AggressiveInlining)]
		public static JSValue JS_NewInt64(JSContext ctx, long val)
		{
			JSValue v;
			if (val == unchecked((int)val)) {
				v = JS_NewInt32(ctx, unchecked((int)val));
			} else {
				v = JSValue.__JS_NewFloat64(ctx, val);
			}
			return v;
		}

		/// <summary>
		/// Creates a <see cref="JSValue"/> holding a 32-bit unsigned integer.
		/// </summary>
		/// <param name="ctx">A pointer to the native JSContext in which the value will be created.</param>
		/// <param name="val">A 32-bit unsigned integer.</param>
		/// <returns>A <see cref="JSValue"/> holding a 32-bit unsigned integer.</returns>
		[MethodImpl(AggressiveInlining)]
		public static JSValue JS_NewUint32(JSContext ctx, uint val)
		{
			JSValue v;
			if (val <= 0x7fffffff)
			{
				v = JS_NewInt32(ctx, (int)val);
			}
			else
			{
				v = JSValue.__JS_NewFloat64(ctx, val);
			}
			return v;
		}

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewBigInt64(JSContext ctx, long v);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewBigUint64(JSContext ctx, ulong v);

		/// <summary>
		/// Creates a value holding a double-precision floating-point number.
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="d"></param>
		/// <returns></returns>
		[MethodImpl(AggressiveInlining)]
		public static JSValue JS_NewFloat64(JSContext ctx, double d)
		{
			int val = (int)d;
			var u = new JSValue();
			u.float64 = d;
			var t = new JSValue();
			t.float64 = val;
			// -0 cannot be represented as integer, so we compare the bit representation
			if (u.uint64 == t.uint64)
			{
				return JSValue.JS_MKVAL(JSTag.Int, val);
			}
			else
			{
				return JSValue.__JS_NewFloat64(ctx, d);
			}
		}

		/// <summary>
		/// Determine if a given <see cref="JSValue"/> is a JavaScript number.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		[MethodImpl(AggressiveInlining)]
		public static bool JS_IsNumber([In] JSValue v)
		{
			JSTag tag = v.Tag;
			return tag == JSTag.Int || tag == JSTag.Float64;
		}

		/// <summary>
		/// Determine if a given <see cref="JSValue"/> is a JavaScript number
		/// represented in memory as an integer or a BigInt.
		/// </summary>
		/// <param name="v">The <see cref="JSValue"/> to test.</param>
		/// <returns>true if the value <paramref name="v"/> is holding an integer type; otherwise, false.</returns>
		[MethodImpl(AggressiveInlining)]
		public static bool JS_IsInteger(JSValue v)
		{
			return v._tagdata.tag == JSTag.Int || v._tagdata.tag == JSTag.BigInt;
		}

		/// <summary>
		/// Determine if a given <see cref="JSValue"/> is a JavaScript number
		/// represented in memory as a BigFloat.
		/// </summary>
		/// <param name="v">The <see cref="JSValue"/> to test.</param>
		/// <returns>true if the value <paramref name="v"/> is holding a BigFloat type; otherwise, false.</returns>
		[MethodImpl(AggressiveInlining)]
		public static bool JS_IsBigFloat(JSValue v)
		{
			return v._tagdata.tag == JSTag.BigFloat;
		}

		/// <summary>
		/// Determine if a given <see cref="JSValue"/> is a JavaScript number
		/// represented in memory as a BigDecimal.
		/// </summary>
		/// <param name="v">The <see cref="JSValue"/> to test.</param>
		/// <returns>true if the value <paramref name="v"/> is holding a BigDecimal type; otherwise, false.</returns>
		[MethodImpl(AggressiveInlining)]
		public static bool JS_IsBigDecimal(JSValue v)
		{
			return v._tagdata.tag == JSTag.BigDecimal;
		}

		/// <summary>
		/// Determines if a given <see cref="JSValue"/> is a JavaScript boolean.
		/// </summary>
		/// <param name="v">The <see cref="JSValue"/> to test.</param>
		/// <returns>true if the value <paramref name="v"/> is holding a Boolean type; otherwise, false.</returns>
		[MethodImpl(AggressiveInlining)]
		public static bool JS_IsBool(JSValue v)
		{
			return v._tagdata.tag == JSTag.Bool;
		}

		/// <summary>
		/// Determine if the specified <see cref="JSValue"/> is null.
		/// </summary>
		/// <param name="v">The <see cref="JSValue"/> to test.</param>
		/// <returns>true if the value <paramref name="v"/> is holding null; otherwise, false.</returns>
		[MethodImpl(AggressiveInlining)]
		public static bool JS_IsNull(JSValue v)
		{
			return v._tagdata.tag == JSTag.Null;
		}

		/// <summary>
		/// Determine if the specified <see cref="JSValue"/> is undefined.
		/// </summary>
		/// <param name="v">The <see cref="JSValue"/> to test.</param>
		/// <returns>
		/// true if the value <paramref name="v"/> is holding the JavaScript
		/// value undefined; otherwise, false.
		/// </returns>
		[MethodImpl(AggressiveInlining)]
		public static bool JS_IsUndefined(JSValue v)
		{
			return v._tagdata.tag == JSTag.Undefined;
		}

		/// <summary>
		/// Determine if the specified <see cref="JSValue"/> is an exception.
		/// </summary>
		/// <param name="v">The <see cref="JSValue"/> to test.</param>
		/// <returns>
		/// true if the value <paramref name="v"/> is <see cref="JSValue.Exception"/>;
		/// otherwise, false.
		/// </returns>
		[MethodImpl(AggressiveInlining)]
		public static bool JS_IsException(JSValue v)
		{
			return v._tagdata.tag == JSTag.Exception;
		}

		/// <summary>
		/// Determine if the specified <see cref="JSValue"/> is uninitialized.
		/// </summary>
		/// <param name="v">The <see cref="JSValue"/> to test.</param>
		/// <returns>
		/// true if the value <paramref name="v"/> is <see cref="JSValue.Uninitialized"/>;
		/// otherwise, false.
		/// </returns>
		[MethodImpl(AggressiveInlining)]
		public static bool JS_IsUninitialized(JSValue v)
		{
			return v._tagdata.tag == JSTag.Uninitialized;
		}

		/// <summary>
		/// Determine if the specified <see cref="JSValue"/> is a string.
		/// </summary>
		/// <param name="v">The <see cref="JSValue"/> to test.</param>
		/// <returns>true if the value <paramref name="v"/> is holding a string type; otherwise, false.</returns>
		[MethodImpl(AggressiveInlining)]
		public static bool JS_IsString(JSValue v)
		{
			return v._tagdata.tag == JSTag.String;
		}

		/// <summary>
		/// Determine if the specified <see cref="JSValue"/> is a Symbol.
		/// </summary>
		/// <param name="v">The <see cref="JSValue"/> to test.</param>
		/// <returns>
		/// true if the value <paramref name="v"/> is holding the JavaScript
		/// Symbol type; otherwise, false.
		/// </returns>
		[MethodImpl(AggressiveInlining)]
		public static bool JS_IsSymbol(JSValue v)
		{
			return v._tagdata.tag == JSTag.Symbol;
		}

		/// <summary>
		/// Determine if the specified <see cref="JSValue"/> is an object.
		/// </summary>
		/// <param name="v">The <see cref="JSValue"/> to test.</param>
		/// <returns>true if the value <paramref name="v"/> is holding an object; otherwise, false.</returns>
		[MethodImpl(AggressiveInlining)]
		public static bool JS_IsObject(JSValue v)
		{
			return v._tagdata.tag == JSTag.Object;
		}

		/// <summary>
		/// Throws a user-defined exception.
		/// </summary>
		/// <param name="ctx">A pointer to the native JSContext.</param>
		/// <param name="obj">
		/// The expression to throw. WARNING: <paramref name="obj"/> is freed.
		/// </param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_Throw(JSContext ctx, JSValue obj);

		/// <summary>
		/// Returns the current pending exception for a given JSContext.
		/// </summary>
		/// <param name="ctx">
		/// A pointer to the native JSContext in which the exception was thrown.
		/// </param>
		/// <returns>
		/// If an exception has been thrown in the context, and it has not yet been caught or cleared,
		/// returns the exception; otherwise return <see cref="JSValue.Null"/>.
		/// </returns>
		/// <remarks>Cannot be called twice.</remarks>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_GetException(JSContext ctx);

		/// <summary>
		/// Determine if the specified <see cref="JSValue"/> is an Error object.
		/// </summary>
		/// <param name="ctx">The pointer to the context that the <see cref="JSValue"/> belongs to.</param>
		/// <param name="val">The <see cref="JSValue"/> to test.</param>
		/// <returns>true if the value <paramref name="val"/> is holding an Error object; otherwise, false.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool JS_IsError(JSContext ctx, [In] JSValue val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_EnableIsErrorProperty(JSContext ctx, [MarshalAs(UnmanagedType.Bool)] bool enable);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_ResetUncatchableError(JSContext ctx);

		/// <summary>
		/// Creates a new <see cref="JSValue"/> which is an Error object.
		/// </summary>
		/// <param name="ctx">
		/// A pointer to the native JSContext in which the value will be created.
		/// </param>
		/// <returns>A <see cref="JSValue"/> instance representing an error.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewError(JSContext ctx);

		/// <summary>
		/// Creates and throws the SyntaxError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="fmt">An ASCII string specifying how to interpret the data.</param>
		/// <param name="__arglist">Arguments specifying data to format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_ThrowSyntaxError(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string fmt, __arglist);

		/// <summary>
		/// Creates and throws the SyntaxError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="fmt">A pointer to a null-terminated multibyte string specifying how to interpret the data.</param>
		/// <param name="__arglist">Arguments specifying data to format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public unsafe static extern JSValue JS_ThrowSyntaxError(JSContext ctx, byte* fmt, __arglist);

		/// <summary>
		/// Creates and throws the SyntaxError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="message">The message that describes the error.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe static JSValue JS_ThrowSyntaxError(JSContext ctx, string message)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			fixed (byte* fmt = Utils.StringToManagedUTF8(message.Replace("%", "%%")))
			{
				return JS_ThrowSyntaxError(ctx, fmt, __arglist());
			}
		}

		/// <summary>
		/// Creates and throws the SyntaxError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An array of objects to write using format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe static JSValue JS_ThrowSyntaxError(JSContext ctx, string format, params object[] args)
		{
			if (format == null)
				throw new ArgumentNullException(nameof(format));

			fixed (byte* fmt = Utils.StringToManagedUTF8(string.Format(format, args).Replace("%", "%%")))
			{
				return JS_ThrowSyntaxError(ctx, fmt, __arglist());
			}
		}

		/// <summary>
		/// Creates and throws the TypeError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="fmt">An ASCII string specifying how to interpret the data.</param>
		/// <param name="__arglist">Arguments specifying data to format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_ThrowTypeError(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string fmt, __arglist);

		/// <summary>
		/// Creates and throws the TypeError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="fmt">A pointer to a null-terminated multibyte string specifying how to interpret the data.</param>
		/// <param name="__arglist">Arguments specifying data to format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_ThrowTypeError(JSContext ctx, byte* fmt, __arglist);

		/// <summary>
		/// Creates and throws the TypeError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="message">The message that describes the error.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe static JSValue JS_ThrowTypeError(JSContext ctx, string message)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			fixed (byte* fmt = Utils.StringToManagedUTF8(message.Replace("%", "%%")))
			{
				return JS_ThrowTypeError(ctx, fmt, __arglist());
			}
		}

		/// <summary>
		/// Creates and throws the TypeError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An array of objects to write using format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe static JSValue JS_ThrowTypeError(JSContext ctx, string format, params object[] args)
		{
			if (format == null)
				throw new ArgumentNullException(nameof(format));

			fixed (byte* fmt = Utils.StringToManagedUTF8(string.Format(format, args).Replace("%", "%%")))
			{
				return JS_ThrowTypeError(ctx, fmt, __arglist());
			}
		}

		/// <summary>
		/// Creates and throws the ReferenceError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="fmt">An ASCII string specifying how to interpret the data.</param>
		/// <param name="__arglist">Arguments specifying data to format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_ThrowReferenceError(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string fmt, __arglist);

		/// <summary>
		/// Creates and throws the ReferenceError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="fmt">A pointer to a null-terminated multibyte string specifying how to interpret the data.</param>
		/// <param name="__arglist">Arguments specifying data to format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_ThrowReferenceError(JSContext ctx, byte* fmt, __arglist);

		/// <summary>
		/// Creates and throws the ReferenceError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="message">The message that describes the error.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe static JSValue JS_ThrowReferenceError(JSContext ctx, string message)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			fixed (byte* fmt = Utils.StringToManagedUTF8(message.Replace("%", "%%")))
			{
				return JS_ThrowTypeError(ctx, fmt, __arglist());
			}
		}

		/// <summary>
		/// Creates and throws the ReferenceError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An array of objects to write using format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe static JSValue JS_ThrowReferenceError(JSContext ctx, string format, params object[] args)
		{
			if (format == null)
				throw new ArgumentNullException(nameof(format));

			fixed (byte* fmt = Utils.StringToManagedUTF8(string.Format(format, args).Replace("%", "%%")))
			{
				return JS_ThrowReferenceError(ctx, fmt, __arglist());
			}
		}

		/// <summary>
		/// Creates and throws the RangeError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="fmt">An ASCII string specifying how to interpret the data.</param>
		/// <param name="__arglist">Arguments specifying data to format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_ThrowRangeError(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string fmt, __arglist);

		/// <summary>
		/// Creates and throws the RangeError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="fmt">A pointer to a null-terminated multibyte string specifying how to interpret the data.</param>
		/// <param name="__arglist">Arguments specifying data to format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_ThrowRangeError(JSContext ctx, byte* fmt, __arglist);

		/// <summary>
		/// Creates and throws the RangeError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="message">The message that describes the error.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe static JSValue JS_ThrowRangeError(JSContext ctx, string message)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			fixed (byte* fmt = Utils.StringToManagedUTF8(message.Replace("%", "%%")))
			{
				return JS_ThrowRangeError(ctx, fmt, __arglist());
			}
		}

		/// <summary>
		/// Creates and throws the RangeError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An array of objects to write using format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe static JSValue JS_ThrowRangeError(JSContext ctx, string format, params object[] args)
		{
			if (format == null)
				throw new ArgumentNullException(nameof(format));

			fixed (byte* fmt = Utils.StringToManagedUTF8(string.Format(format, args).Replace("%", "%%")))
			{
				return JS_ThrowRangeError(ctx, fmt, __arglist());
			}
		}

		/// <summary>
		/// Creates and throws the InternalError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="fmt">An ASCII string specifying how to interpret the data.</param>
		/// <param name="__arglist">Arguments specifying data to format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_ThrowInternalError(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string fmt, __arglist);

		/// <summary>
		/// Creates and throws the InternalError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="fmt">A pointer to a null-terminated multibyte string specifying how to interpret the data.</param>
		/// <param name="__arglist">Arguments specifying data to format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_ThrowInternalError(JSContext ctx, byte* fmt, __arglist);

		/// <summary>
		/// Creates and throws the InternalError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="message">The message that describes the error.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe static JSValue JS_ThrowInternalError(JSContext ctx, string message)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			fixed (byte* fmt = Utils.StringToManagedUTF8(message.Replace("%", "%%")))
			{
				return JS_ThrowInternalError(ctx, fmt, __arglist());
			}
		}

		/// <summary>
		/// Creates and throws the InternalError.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An array of objects to write using format.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe static JSValue JS_ThrowInternalError(JSContext ctx, string format, params object[] args)
		{
			if (format == null)
				throw new ArgumentNullException(nameof(format));

			fixed (byte* fmt = Utils.StringToManagedUTF8(string.Format(format, args).Replace("%", "%%")))
			{
				return JS_ThrowInternalError(ctx, fmt, __arglist());
			}
		}

		/// <summary>
		/// Reports a memory allocation error.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the exception was thrown.</param>
		/// <returns>The <see cref="JSValue.Exception"/> value.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_ThrowOutOfMemory(JSContext ctx);

		/// <summary>
		/// Deallocates a memory previously allocated to the specified ref-counted <see cref="JSValue"/>.
		/// </summary>
		/// <param name="ctx">A pointer to the context that <see cref="JSValue"/> <paramref name="v"/> belongs to.</param>
		/// <param name="v">The <see cref="JSValue"/> to free.</param>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void __JS_FreeValue(JSContext ctx, JSValue v);

		/// <summary>
		/// Decrements the reference count on the specified <see cref="JSValue"/>.
		/// </summary>
		/// <param name="ctx">A pointer to the context that <see cref="JSValue"/> <paramref name="v"/> belongs to.</param>
		/// <param name="v">The <see cref="JSValue"/> to free.</param>
		[MethodImpl(AggressiveInlining)]
		public unsafe static void JS_FreeValue(JSContext ctx, JSValue v)
		{
			if ((uint)v._tagdata.tag >= unchecked((uint)JSTag.First))
			{
				JSRefCountHeader* p = (JSRefCountHeader*)v.ptr;
				if (--p->ref_count <= 0)
				{
					__JS_FreeValue(ctx, v);
				}
			}
		}

		/// <summary>
		/// Deallocates a memory previously allocated to the specified ref-counted <see cref="JSValue"/>.
		/// </summary>
		/// <param name="rt">A pointer to the runtime that <see cref="JSValue"/> <paramref name="v"/> belongs to.</param>
		/// <param name="v">The <see cref="JSValue"/> to free.</param>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void __JS_FreeValueRT(JSRuntime rt, JSValue v);

		/// <summary>
		/// Decrements the reference count on the specified <see cref="JSValue"/>.
		/// </summary>
		/// <param name="rt">A pointer to the runtime that <see cref="JSValue"/> <paramref name="v"/> belongs to.</param>
		/// <param name="v">The <see cref="JSValue"/> to free.</param>
		[MethodImpl(AggressiveInlining)]
		public unsafe static void JS_FreeValueRT(JSRuntime rt, JSValue v)
		{
			if ((uint)v._tagdata.tag >= unchecked((uint)JSTag.First))
			{
				JSRefCountHeader* p = (JSRefCountHeader*)v.ptr;
				if (--p->ref_count <= 0)
				{
					__JS_FreeValueRT(rt, v);
				}
			}
		}

		/// <summary>
		/// Increments the reference count on the specified <see cref="JSValue"/>.
		/// </summary>
		/// <param name="ctx">A pointer to the context that <see cref="JSValue"/> <paramref name="v"/> belongs to.</param>
		/// <param name="v">The <see cref="JSValue"/> instance.</param>
		/// <returns>A <see cref="JSValue"/> with an incremented reference count.</returns>
		[MethodImpl(AggressiveInlining)]
		public unsafe static JSValue JS_DupValue(JSContext ctx, JSValue v)
		{
			if ((uint)v._tagdata.tag >= unchecked((uint)JSTag.First))
			{
				JSRefCountHeader* p = (JSRefCountHeader*)v.ptr;
				p->ref_count++;
			}
			return v;
		}

		/// <summary>
		/// Converts the JavaScript value to a <see cref="Boolean"/> value.
		/// </summary>
		/// <param name="ctx">A pointer to the context that <see cref="JSValue"/> belongs to.</param>
		/// <param name="val">The value.</param>
		/// <returns>Non zero - true; 0 - false; -1 for JS_EXCEPTION.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_ToBool(JSContext ctx, [In] JSValue val);

		/// <summary>
		/// Converts the JavaScript value to a 32-bit signed integer.
		/// </summary>
		/// <param name="ctx">
		/// A pointer to the context that the specified <see cref="JSValue"/>
		/// <paramref name="val"/> belongs to.
		/// </param>
		/// <param name="value">
		/// When this method returns, contains the 32-bit signed integer value equivalent
		/// of the number contained in <paramref name="val"/>.
		/// </param>
		/// <param name="val">The value.</param>
		/// <returns>(&lt;0, 0) in case of exception</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_ToInt32(JSContext ctx, out int value, [In] JSValue val);

		/// <summary>
		/// Converts the JavaScript value to a 32-bit unsigned integer.
		/// </summary>
		/// <param name="ctx">
		/// A pointer to the context that the specified <see cref="JSValue"/>
		/// <paramref name="val"/> belongs to.
		/// </param>
		/// <param name="value">
		/// When this method returns, contains the 32-bit unsigned integer value equivalent
		/// of the number contained in <paramref name="val"/>.
		/// </param>
		/// <param name="val">The value.</param>
		/// <returns>(&lt;0, 0) in case of exception</returns>
		[MethodImpl(AggressiveInlining)]
		public static int JS_ToUint32(JSContext ctx, out uint value, [In] JSValue val)
		{
			int rv = JS_ToInt32(ctx, out int v, val);
			value = unchecked((uint)v);
			return rv;
		}

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_ToInt64(JSContext ctx, out long value, [In] JSValue val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_ToIndex(JSContext ctx, out ulong len, [In] JSValue val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_ToFloat64(JSContext ctx, out double value, [In] JSValue val);

		/* return an exception if 'val' is a Number */
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_ToBigInt64(JSContext ctx, out long pres, [In] JSValue val);

		/* same as JS_ToInt64() but allow BigInt */
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_ToInt64Ext(JSContext ctx, out long pres, [In] JSValue val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_NewStringLen(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string s, SizeT len);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_NewStringLen(JSContext ctx, byte* str, SizeT len);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_NewString(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string s);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_NewAtomString(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string s);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public unsafe static extern JSValue JS_NewAtomString(JSContext ctx, byte* str);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_ToString(JSContext ctx, [In] JSValue val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_ToPropertyKey(JSContext ctx, [In] JSValue val);

		/// <summary>
		/// Converts the value of the specified <see cref="JSValue"/> to its equivalent UTF-8 string representation.
		/// </summary>
		/// <param name="ctx">The pointer to the context that the <see cref="JSValue"/> belongs to.</param>
		/// <param name="len">When the method returns, a value containing the length of the UTF-8 string in bytes.</param>
		/// <param name="val">A <see cref="JSValue"/> that supplies the value to convert.</param>
		/// <param name="cesu8">Determines if non-BMP1 codepoints are encoded as 1 or 2 utf-8 sequences (CESU-8 encoding).</param>
		/// <returns>A pointer into a JSString with a live ref_count if the operation is successful; NULL, if exception.</returns>
		/// <remarks>
		/// Because this method allocates the unmanaged memory required for a string,
		/// always free the memory by calling <see cref="JS_FreeCString"/>.
		/// </remarks>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr JS_ToCStringLen2(JSContext ctx, out SizeT len, [In] JSValue val, [MarshalAs(UnmanagedType.Bool)] bool cesu8);

		/// <summary>
		/// Converts the value of the specified <see cref="JSValue"/> to its equivalent UTF-8 string representation.
		/// </summary>
		/// <param name="ctx">The pointer to the context that the <see cref="JSValue"/> belongs to.</param>
		/// <param name="len">When the method returns, a value containing the length of the UTF-8 string in bytes.</param>
		/// <param name="val">A <see cref="JSValue"/> that supplies the value to convert.</param>
		/// <returns>A pointer into a JSString with a live ref_count if the operation is successful; NULL, if exception.</returns>
		/// <remarks>
		/// Because this method allocates the unmanaged memory required for a string,
		/// always free the memory by calling <see cref="JS_FreeCString"/>.
		/// </remarks>
		[MethodImpl(AggressiveInlining)]
		public static IntPtr JS_ToCStringLen(JSContext ctx, out SizeT len, [In] JSValue val)
		{
			return JS_ToCStringLen2(ctx, out len, val, false);
		}

		/// <summary>
		/// Converts the value of the specified <see cref="JSValue"/> to its equivalent UTF-8 null-terminated string representation.
		/// </summary>
		/// <param name="ctx">The pointer to the context that the <see cref="JSValue"/> belongs to.</param>
		/// <param name="val">A <see cref="JSValue"/> that supplies the value to convert.</param>
		/// <returns>A pointer into a JSString with a live ref_count if the operation is successful; NULL, if exception.</returns>
		/// <remarks>
		/// Because this method allocates the unmanaged memory required for a string,
		/// always free the memory by calling <see cref="JS_FreeCString"/>.
		/// </remarks>
		[MethodImpl(AggressiveInlining)]
		public static IntPtr JS_ToCString(JSContext ctx, [In] JSValue val)
		{
			return JS_ToCStringLen2(ctx, out SizeT len, val, false);
		}

		/// <summary>
		/// Frees memory previously allocated for a string.
		/// </summary>
		/// <param name="ctx">A pointer to the context in which the memory was allocated for the string.</param>
		/// <param name="ptr">A pointer to the string.</param>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_FreeCString(JSContext ctx, IntPtr ptr);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewObjectProtoClass(JSContext ctx, [In] JSValue proto, JSClassID class_id);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewObjectClass(JSContext ctx, JSClassID class_id);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewObjectProto(JSContext ctx, [In] JSValue proto);

		/// <summary>
		/// Creates a new JavaScript object.
		/// </summary>
		/// <param name="ctx">The context in which to create the new object.</param>
		/// <returns>
		/// On success, returns the new JavaScript object.
		/// Otherwise it returns <see cref="JSValue.Exception"/>.
		/// </returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewObject(JSContext ctx);

		/// <summary>
		/// Test whether a given value is a Function.
		/// </summary>
		/// <param name="ctx">The pointer to the context that the <see cref="JSValue"/> belongs to.</param>
		/// <param name="val">The <see cref="JSValue"/> to test.</param>
		/// <returns>true if <paramref name="val"/> is a Function and false otherwise.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool JS_IsFunction(JSContext ctx, [In] JSValue val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool JS_IsConstructor(JSContext ctx, [In] JSValue val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool JS_SetConstructorBit(JSContext ctx, [In] JSValue func_obj, [MarshalAs(UnmanagedType.Bool)] bool val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewArray(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_IsArray(JSContext ctx, [In] JSValue val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_GetPropertyInternal(JSContext ctx, [In] JSValue obj, JSAtom prop, [In] JSValue receiver, [MarshalAs(UnmanagedType.Bool)] bool throw_ref_error);

		[MethodImpl(AggressiveInlining)]
		public static JSValue JS_GetProperty(JSContext ctx, [In] JSValue this_obj, JSAtom prop)
		{
			return JS_GetPropertyInternal(ctx, this_obj, prop, this_obj, false);
		}

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_GetPropertyStr(JSContext ctx, [In] JSValue this_obj, [MarshalAs(UnmanagedType.LPStr)] string prop);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_GetPropertyStr(JSContext ctx, [In] JSValue this_obj, byte* prop);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_GetPropertyUint32(JSContext ctx, [In] JSValue this_obj, uint idx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_SetPropertyInternal(JSContext ctx, [In] JSValue this_obj, JSAtom prop, JSValue val, JSPropertyFlags flags);

		[MethodImpl(AggressiveInlining)]
		public static int JS_SetProperty(JSContext ctx, [In] JSValue this_obj, JSAtom prop, JSValue val)
		{
			return JS_SetPropertyInternal(ctx, this_obj, prop, val, JSPropertyFlags.Throw);
		}

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_SetPropertyUint32(JSContext ctx, [In] JSValue this_obj, uint idx, JSValue val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_SetPropertyInt64(JSContext ctx, [In] JSValue this_obj, long idx, JSValue val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern int JS_SetPropertyStr(JSContext ctx, [In] JSValue this_obj, [MarshalAs(UnmanagedType.LPStr)] string prop, JSValue val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern int JS_SetPropertyStr(JSContext ctx, [In] JSValue this_obj, byte* prop, JSValue val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_HasProperty(JSContext ctx, [In] JSValue this_obj, JSAtom prop);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_IsExtensible(JSContext ctx, [In] JSValue obj);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_PreventExtensions(JSContext ctx, [In] JSValue obj);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_DeleteProperty(JSContext ctx, [In] JSValue obj, JSAtom prop, JSPropertyFlags flags);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_SetPrototype(JSContext ctx, [In] JSValue obj, [In] JSValue proto_val);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_GetPrototype(JSContext ctx, [In] JSValue val);

		/// <summary>
		/// Gets an array of all properties found directly in a given object.
		/// </summary>
		/// <param name="ctx">A pointer to the JavaScript context.</param>
		/// <param name="ptab">
		/// When the method returns, the pointer to an array of <see cref="JSPropertyEnum"/>&apos;s.
		/// </param>
		/// <param name="len">The size of an array.</param>
		/// <param name="obj">
		/// The object whose enumerable and non-enumerable properties are to be returned.
		/// </param>
		/// <param name="flags">A bitwise combination of <see cref="JSGetPropertyNamesFlags"/> values.</param>
		/// <returns>0 if OK, -1 if exception.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern int JS_GetOwnPropertyNames(JSContext ctx, JSPropertyEnum** ptab, out uint len, [In] JSValue obj, JSGetPropertyNamesFlags flags);

		/// <summary>
		/// Gets an array of all properties found directly in a given object.
		/// </summary>
		/// <param name="ctx">A pointer to the JavaScript context.</param>
		/// <param name="props">
		/// When the method returns, the array of <see cref="JSPropertyEnum"/>&apos;s.
		/// </param>
		/// <param name="obj">
		/// The object whose enumerable and non-enumerable properties are to be returned.
		/// </param>
		/// <param name="flags">A bitwise combination of <see cref="JSGetPropertyNamesFlags"/> values.</param>
		/// <returns>0 if OK, -1 if exception.</returns>
		public unsafe static int JS_GetOwnPropertyNames(JSContext ctx, out JSPropertyEnum[] props, JSValue obj, JSGetPropertyNamesFlags flags)
		{
			uint len;
			props = null;
			JSPropertyEnum* ptab = null;
			int rv = JS_GetOwnPropertyNames(ctx, &ptab, out len, obj, flags);
			if (ptab == null)
				return rv;
			props = new JSPropertyEnum[len];
			for (int i = 0; i < props.Length; i++)
			{
				props[i] = *(ptab + i);
			}
			js_free(ctx, new IntPtr(ptab));
			return rv;
		}

		/// <summary>
		/// Gets a detailed description of a property.
		/// </summary>
		/// <param name="ctx">The JavaScript context.</param>
		/// <param name="desc">A handle to a descriptor of the property or <see cref="JSPropertyDescriptorHandle.Zero"/>.</param>
		/// <param name="obj">The object to search for the property.</param>
		/// <param name="prop">The name of the property to look up.</param>
		/// <returns></returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_GetOwnProperty(JSContext ctx, JSPropertyDescriptorHandle desc, JSValue obj, JSAtom prop);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_Call(JSContext ctx, [In] JSValue func_obj, [In] JSValue this_obj, int argc, JSValue* argv);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_Call(JSContext ctx, [In] JSValue func_obj, [In] JSValue this_obj, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] JSValue[] argv);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_Invoke(JSContext ctx, [In] JSValue this_val, JSAtom atom, int argc, JSValue* argv);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_Invoke(JSContext ctx, [In] JSValue this_val, JSAtom atom, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] JSValue[] argv);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_CallConstructor(JSContext ctx, [In] JSValue func_obj, int argc, JSValue* argv);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_CallConstructor(JSContext ctx, [In] JSValue func_obj, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] JSValue[] argv);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_CallConstructor2(JSContext ctx, [In] JSValue func_obj, [In] JSValue new_target, int argc, JSValue* argv);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_CallConstructor2(JSContext ctx, [In] JSValue func_obj, [In] JSValue new_target, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] JSValue[] argv);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public unsafe static extern bool JS_DetectModule([MarshalAs(UnmanagedType.LPStr)] string input, SizeT input_len);

		/* 'input' must be zero terminated i.e. input[input_len] = '\0'. */
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public unsafe static extern JSValue JS_Eval(JSContext ctx, byte* input, SizeT input_len, [MarshalAs(UnmanagedType.LPStr)] string filename, JSEvalFlags eval_flags);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_Eval(JSContext ctx, byte* input, SizeT input_len, byte* filename, JSEvalFlags eval_flags);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_EvalFunction(JSContext ctx, JSValue fun_obj);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_GetGlobalObject(JSContext ctx);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_IsInstanceOf(JSContext ctx, [In] JSValue val, [In] JSValue obj);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_DefineProperty(JSContext ctx, [In] JSValue this_obj, JSAtom prop, [In] JSValue val, [In] JSValue getter, [In] JSValue setter, JSPropertyFlags flags);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_DefinePropertyValue(JSContext ctx, [In] JSValue this_obj, JSAtom prop, JSValue val, JSPropertyFlags flags);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_DefinePropertyValueUint32(JSContext ctx, [In] JSValue this_obj, uint idx, JSValue val, JSPropertyFlags flags);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="this_obj"></param>
		/// <param name="prop"></param>
		/// <param name="val"></param>
		/// <param name="flags"></param>
		/// <returns>return -1 (exception), FALSE (0) or TRUE.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern int JS_DefinePropertyValueStr(JSContext ctx, [In] JSValue this_obj, [MarshalAs(UnmanagedType.LPStr)] string prop, JSValue val, JSPropertyFlags flags);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern int JS_DefinePropertyValueStr(JSContext ctx, [In] JSValue this_obj, byte* prop, JSValue val, JSPropertyFlags flags);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_DefinePropertyGetSet(JSContext ctx, [In] JSValue this_obj, JSAtom prop, JSValue getter, JSValue setter, JSPropertyFlags flags);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_SetOpaque(JSValue obj, IntPtr opaque);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr JS_GetOpaque([In] JSValue obj, JSClassID class_id);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr JS_GetOpaque2(JSContext ctx, [In] JSValue obj, JSClassID class_id);

		/// <summary>
		/// Parses the specified buffer as JSON string, constructing the JavaScript value or object described by the string.
		/// </summary>
		/// <param name="ctx">A pointer to the JavaScript context.</param>
		/// <param name="buffer">
		/// An unsafe pointer to a buffer that contains a JSON string.
		/// Must be zero terminated (i.e. buffer[bufferSize] = &apos;\0&apos;).
		/// </param>
		/// <param name="bufferSize">The size of the buffer.</param>
		/// <param name="filename">The name of the JSON file.</param>
		/// <returns>The new <see cref="JSValue"/> corresponding to the given JSON text or <see cref="JSValue.Exception"/>.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_ParseJSON(JSContext ctx, byte* buffer, SizeT bufferSize, byte* filename);

		/// <summary>
		/// Parses the specified buffer as JSON string, constructing the JavaScript value or object described by the string.
		/// </summary>
		/// <param name="ctx">A pointer to the JavaScript context.</param>
		/// <param name="buffer">
		/// An unsafe pointer to a buffer that contains a JSON string.
		/// Must be zero terminated (i.e. buffer[bufferSize] = &apos;\0&apos;).
		/// </param>
		/// <param name="bufferSize">The size of the buffer.</param>
		/// <param name="filename">The name of the JSON file.</param>
		/// <returns>The new <see cref="JSValue"/> corresponding to the given JSON text or <see cref="JSValue.Exception"/>.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public unsafe static extern JSValue JS_ParseJSON(JSContext ctx, byte* buffer, SizeT bufferSize, [MarshalAs(UnmanagedType.LPStr)] string filename);

		/// <summary>
		/// Parses the specified buffer as JSON string, constructing the JavaScript value or object.
		/// </summary>
		/// <param name="ctx">A pointer to the JavaScript context.</param>
		/// <param name="buffer">
		/// A pointer to a buffer that contains a JSON string.
		/// Must be zero terminated (i.e. buffer[bufferSize] = &apos;\0&apos;).
		/// </param>
		/// <param name="bufferSize">The size of the buffer.</param>
		/// <param name="filename">The name of the JSON file.</param>
		/// <returns>The new <see cref="JSValue"/> corresponding to the given JSON text or <see cref="JSValue.Exception"/>.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_ParseJSON(JSContext ctx, IntPtr buffer, SizeT bufferSize, [MarshalAs(UnmanagedType.LPStr)] string filename);

		/// <summary>
		/// Parses the specified buffer as JSON string, constructing the JavaScript value or object described by the string.
		/// </summary>
		/// <param name="ctx">A pointer to the JavaScript context.</param>
		/// <param name="buffer">
		/// The buffer that contains a JSON string.
		/// Must be zero terminated (i.e. buffer[bufferSize] = &apos;\0&apos;).
		/// </param>
		/// <param name="bufferSize">The size of the buffer reduced by 1.</param>
		/// <param name="filename">The name of the JSON file.</param>
		/// <returns>The new <see cref="JSValue"/> corresponding to the given JSON text or <see cref="JSValue.Exception"/>.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_ParseJSON(JSContext ctx, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] buffer, SizeT bufferSize, [MarshalAs(UnmanagedType.LPStr)] string filename);

		/// <summary>
		/// Parses the specified JSON string, constructing the JavaScript value or object described by the string.
		/// </summary>
		/// <param name="ctx">A pointer to the JavaScript context.</param>
		/// <param name="json">The string to parse as JSON.</param>
		/// <param name="bufferSize">The size of the buffer reduced by 1.</param>
		/// <param name="filename">The name of the JSON file.</param>
		/// <returns>The new <see cref="JSValue"/> corresponding to the given JSON text or <see cref="JSValue.Exception"/>.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_ParseJSON(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string json, SizeT len, [MarshalAs(UnmanagedType.LPStr)] string filename);

		/// <summary>
		/// Converts a JavaScript object or value to a JSON string, optionally replacing values
		/// if a replacer function is specified or optionally including only the specified
		/// properties if a replacer array is specified.
		/// </summary>
		/// <param name="ctx">A pointer to the JavaScript context.</param>
		/// <param name="obj">The value to convert to a JSON string.</param>
		/// <param name="replacer">
		/// A function that alters the behavior of the stringification process, or an array
		/// of String and Number that serve as a whitelist for selecting/filtering the properties
		/// of the value object to be included in the JSON string.<para/>
		/// If this value is null or not provided, all properties of the object are included
		/// in the resulting JSON string.
		/// </param>
		/// <param name="space">
		/// A String or Number object that&apos;s used to insert white space into the output
		/// JSON string for readability purposes.<para/>
		/// If this is a Number, it indicates the number of space characters to use as white
		/// space; this number is capped at 10 (if it is greater, the value is just 10).
		/// Values less than 1 indicate that no space should be used.<para/>
		/// If this is a String, the string (or the first 10 characters of the string, if
		/// it&apos;s longer than that) is used as white space. If this parameter is
		/// not provided (or is null), no white space is used.
		/// </param>
		/// <returns>A JSON string representing the specified value.</returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_JSONStringify(JSContext ctx, [In] JSValue obj, [In] JSValue replacer, [In] JSValue space);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewArrayBuffer(JSContext ctx, IntPtr buffer, SizeT len, JSFreeArrayBufferDataFunc free_func, IntPtr opaque, [MarshalAs(UnmanagedType.Bool)] bool is_shared);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewArrayBufferCopy(JSContext ctx, IntPtr buffer, SizeT len);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_NewArrayBufferCopy(JSContext ctx, byte* buf, SizeT len);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_DetachArrayBuffer(JSContext ctx, [In] JSValue obj);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr JS_GetArrayBuffer(JSContext ctx, out SizeT size, [In] JSValue obj);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_GetTypedArrayBuffer(JSContext ctx, [In] JSValue obj, out SizeT byte_offset, out SizeT byte_length, out SizeT bytes_per_element);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_NewPromiseCapability(JSContext ctx, JSValue* resolving_funcs);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern void JS_SetHostPromiseRejectionTracker(JSRuntime rt, JSHostPromiseRejectionTracker cb, void* opaque);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_SetInterruptHandler(JSRuntime rt, JSInterruptHandler cb, IntPtr opaque);

		/// <summary>
		/// if can_block is TRUE, Atomics.wait() can be used
		/// </summary>
		/// <param name="rt"></param>
		/// <param name="can_block"></param>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_SetCanBlock(JSRuntime rt, [MarshalAs(UnmanagedType.Bool)] bool can_block);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rt"></param>
		/// <param name="module_normalize">NULL is allowed and invokes the default module filename normalizer.</param>
		/// <param name="module_loader"></param>
		/// <param name="opaque"></param>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_SetModuleLoaderFunc(JSRuntime rt, JSModuleNormalizeFunc module_normalize, JSModuleLoaderFunc module_loader, IntPtr opaque);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rt"></param>
		/// <param name="module_normalize">NULL is allowed and invokes the default module filename normalizer.</param>
		/// <param name="module_loader"></param>
		/// <param name="opaque"></param>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_SetModuleLoaderFunc(JSRuntime rt, IntPtr module_normalize, IntPtr module_loader, IntPtr opaque);

		/* return the import.meta object of a module */
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_GetImportMeta(JSContext ctx, ref JSModuleDef m);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSAtom JS_GetModuleName(JSContext ctx, in JSModuleDef m);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_EnqueueJob(JSContext ctx, JSJobFunc job_func, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] JSValue[] argv);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_EnqueueJob(JSContext ctx, JSJobFunc32 job_func, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] JSValue[] argv);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_EnqueueJob(JSContext ctx, IntPtr job_func, int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] JSValue[] argv);


		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool JS_IsJobPending(JSRuntime rt);

		/// <summary>
		/// Executes a pending job.
		/// </summary>
		/// <param name="rt"></param>
		/// <param name="ctx"></param>
		/// <returns>
		/// Return &lt; 0 if exception, 0 if no job pending, 1 if a job was executed successfully.
		/// The context of the job is stored in <paramref name="ctx"/>.
		/// </returns>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_ExecutePendingJob(JSRuntime rt, out JSContext ctx);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="bufferSize"></param>
		/// <param name="obj"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		/// <remarks>Object writer currently only used to handle precompiled code.</remarks>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr JS_WriteObject(JSContext ctx, out SizeT bufferSize, [In] JSValue obj, JSReaderWriterFlags flags);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="buffer"></param>
		/// <param name="bufferSize"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		/// <remarks>Object reader currently only used to handle precompiled code.</remarks>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_ReadObject(JSContext ctx, IntPtr buffer, SizeT bufferSize, JSReaderWriterFlags flags);

		/* load the dependencies of the module 'obj'. Useful when JS_ReadObject() returns a module. */
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_ResolveModule(JSContext ctx, [In] JSValue obj);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSValue JS_NewCFunction2(JSContext ctx, JSCFunction func, [MarshalAs(UnmanagedType.LPStr)] string name, int length, JSCFunctionEnum cproto, int magic);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewCFunction2(JSContext ctx, JSCFunction func, IntPtr name, int length, JSCFunctionEnum cproto, int magic);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_NewCFunction2(JSContext ctx, JSCFunction func, byte* name, int length, JSCFunctionEnum cproto, int magic);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public unsafe static extern JSValue JS_NewCFunction2(JSContext ctx, IntPtr func, byte* name, int length, JSCFunctionEnum cproto, int magic);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, EntryPoint = "JS_NewCFunction2", CharSet = CharSet.Ansi)]
		public static extern JSValue JS_NewCFunctionMagic64(JSContext ctx, JSCFunctionMagic func, [MarshalAs(UnmanagedType.LPStr)] string asciiname, int length, JSCFunctionEnum cproto, int magic);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, EntryPoint = "JS_NewCFunction2")]
		public unsafe static extern JSValue JS_NewCFunctionMagic64(JSContext ctx, JSCFunctionMagic func, byte* name, int length, JSCFunctionEnum cproto, int magic);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, EntryPoint = "JS_NewCFunction2")]
		public unsafe static extern JSValue JS_NewCFunctionMagic64(JSContext ctx, JSCFunctionMagic func, IntPtr name, int length, JSCFunctionEnum cproto, int magic);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, EntryPoint = "JS_NewCFunction2", CharSet = CharSet.Ansi)]
		public static extern JSValue JS_NewCFunctionMagic32(JSContext ctx, JSCFunctionMagic32 func, [MarshalAs(UnmanagedType.LPStr)] string asciiname, int length, JSCFunctionEnum cproto, int magic);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, EntryPoint = "JS_NewCFunction2")]
		public unsafe static extern JSValue JS_NewCFunctionMagic32(JSContext ctx, JSCFunctionMagic32 func, byte* name, int length, JSCFunctionEnum cproto, int magic);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, EntryPoint = "JS_NewCFunction2")]
		public static extern JSValue JS_NewCFunctionMagic32(JSContext ctx, JSCFunctionMagic32 func, IntPtr name, int length, JSCFunctionEnum cproto, int magic);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, EntryPoint = "JS_NewCFunction2")]
		public unsafe static extern JSValue JS_NewCFunctionMagic(JSContext ctx, IntPtr func, byte* name, int length, JSCFunctionEnum cproto, int magic);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_SetConstructor(JSContext ctx, [In] JSValue func_obj, [In] JSValue proto);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewCFunctionData(JSContext ctx, JSCFunctionData func, int length, int magic, int data_len, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] JSValue[] data);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewCFunctionData(JSContext ctx, JSCFunctionData32 func, int length, int magic, int data_len, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] JSValue[] data);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern JSValue JS_NewCFunctionData(JSContext ctx, IntPtr func, int length, int magic, int data_len, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] JSValue[] data);

		[MethodImpl(AggressiveInlining)]
		public static JSValue JS_NewCFunction(JSContext ctx, JSCFunction func, string asciiname, int length)
		{
			return JS_NewCFunction2(ctx, func, asciiname, length, JSCFunctionEnum.Generic, 0);
		}

		[MethodImpl(AggressiveInlining)]
		public unsafe static JSValue JS_NewCFunction(JSContext ctx, JSCFunction func, byte* name, int length)
		{
			return JS_NewCFunction2(ctx, func, name, length, JSCFunctionEnum.Generic, 0);
		}

		[MethodImpl(AggressiveInlining)]
		public unsafe static JSValue JS_NewCFunction(JSContext ctx, IntPtr func, byte* name, int length)
		{
			return JS_NewCFunction2(ctx, func, name, length, JSCFunctionEnum.Generic, 0);
		}

		[MethodImpl(AggressiveInlining)]
		public static JSValue JS_NewCFunction(JSContext ctx, JSCFunction func, IntPtr name, int length)
		{
			return JS_NewCFunction2(ctx, func, name, length, JSCFunctionEnum.Generic, 0);
		}

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern void JS_SetPropertyFunctionList(JSContext ctx, [In] JSValue obj, in JSCFunctionListEntry tab, int len);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern JSModuleDef JS_NewCModule(JSContext ctx, [MarshalAs(UnmanagedType.LPStr)] string name, JSModuleInitFunc func);

		/// <remarks>Can only be called before the module is instantiated</remarks>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern int JS_AddModuleExport(JSContext ctx, JSModuleDef m, [MarshalAs(UnmanagedType.LPStr)] string name);

		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_AddModuleExportList(JSContext ctx, JSModuleDef m, in JSCFunctionListEntry tab, int len);

		/// <remarks>Can only be called after the module is instantiated</remarks>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern int JS_SetModuleExport(JSContext ctx, JSModuleDef m, [MarshalAs(UnmanagedType.LPStr)] string export_name, JSValue val);

		/// <remarks>Can only be called after the module is instantiated</remarks>
		[DllImport("quickjs", CallingConvention = CallingConvention.Cdecl)]
		public static extern int JS_SetModuleExportList(JSContext ctx, JSModuleDef m, in JSCFunctionListEntry tab, int len);

	}


}
