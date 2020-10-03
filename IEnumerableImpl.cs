using System.Collections;
using System.Collections.Generic;

namespace Imperit
{
	public interface IEnumerableImpl<T> : IEnumerable<T>
	{
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
