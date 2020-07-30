using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Imperit.Load
{
    public interface IConvertibleToWith<T, TA>
    {
        T Convert(int i, TA arg);
    }
    public class Loader<T, TK, TA> where T : IConvertibleToWith<TK, TA>
    {
        protected IFile io;
        protected TA arg;
        public Loader(IFile io, TA arg)
        {
            this.io = io;
            this.arg = arg;
        }
        public IEnumerable<TK> Load() => io.Read().Split('\n', StringSplitOptions.RemoveEmptyEntries).Select((line, i) => JsonSerializer.Deserialize<T>(line).Convert(i, arg));
        public TK LoadOne() => JsonSerializer.Deserialize<T>(io.Read()).Convert(0, arg);
    }
}