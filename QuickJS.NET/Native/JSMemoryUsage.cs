using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	[StructLayout(LayoutKind.Sequential)]
	public struct JSMemoryUsage
	{
		public long malloc_size, malloc_limit, memory_used_size;
		public long malloc_count;
		public long memory_used_count;
		public long atom_count, atom_size;
		public long str_count, str_size;
		public long obj_count, obj_size;
		public long prop_count, prop_size;
		public long shape_count, shape_size;
		public long js_func_count, js_func_size, js_func_code_size;
		public long js_func_pc2line_count, js_func_pc2line_size;
		public long c_func_count, array_count;
		public long fast_array_count, fast_array_elements;
		public long binary_object_count, binary_object_size;
	}


}
