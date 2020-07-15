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
        TK Selector(string line, int i)
        {
            var des = JsonSerializer.Deserialize<T>(line);
            return des.Convert(i, arg);
        }
        public IEnumerable<TK> Load()
        {
            var lines = io.Read().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            return lines.Select(Selector);
        }
        public TK LoadOne() => JsonSerializer.Deserialize<T>(io.Read()).Convert(0, arg);
    }
}