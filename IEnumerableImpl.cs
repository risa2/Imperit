using System.Collections;
using System.Collections.Generic;

namespace Imperit
{
    interface IEnumerableImpl<T> : IEnumerable<T>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
