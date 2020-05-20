#if NET20

using System.Collections.Generic;

namespace System
{
	internal delegate TResult Func<TSource, TResult>(TSource arg);
}

namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
	internal sealed class ExtensionAttribute : Attribute { }

}

namespace System.Linq
{
	internal static class Linq20
	{
		public static IEnumerable<char> Skip(this string s, int count)
		{
			foreach (char ch in s)
			{
				if (count > 0)
				{
					count--;
					continue;
				}
				yield return ch;
			}
		}

		public static bool All<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
		{
			foreach (T item in enumerable)
			{
				if (!predicate(item))
					return false;
			}
			return true;
		}

	}
}

#endif
