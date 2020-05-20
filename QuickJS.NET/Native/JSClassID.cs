using System.Runtime.InteropServices;

namespace QuickJS.Native
{
	[StructLayout(LayoutKind.Sequential)]
	public struct JSClassID
	{
		private int _value;

		public int ToInt32()
		{
			return _value;
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		public override string ToString()
		{
			return _value.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj is JSClassID a)
				return a._value == _value;
			return false;
		}
	}


}
