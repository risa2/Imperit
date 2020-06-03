using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Imperit.Load
{
    public interface IConvertibleToWith<K, A>
    {
        K Convert(int i, A arg);
    }
    public class Loader<T, K, A> where T : IConvertibleToWith<K, A>
    {
        protected IFile io;
        protected A arg;
        public Loader(IFile io, A arg)
        {
            this.io = io;
            this.arg = arg;
        }
        public IEnumerable<K> Load() => io.Read().Split('\n', StringSplitOptions.RemoveEmptyEntries).Select((line, i) => JsonSerializer.Deserialize<T>(line).Convert(i, arg));
        public K LoadOne() => JsonSerializer.Deserialize<T>(io.Read()).Convert(0, arg);
    }
    public class Writer<T, K, A> : Loader<T, K, A> where T : IConvertibleToWith<K, A>
    {
        protected Func<K, T> cvt;
        public Writer(IFile input, A arg, Func<K, T> cvt) : base(input, arg) => this.cvt = cvt;
        string ToWrite(IEnumerable<K> e) => string.Join('\n', e.Select(item => JsonSerializer.Serialize(cvt(item), new JsonSerializerOptions() { IgnoreNullValues = true })));
        public void Save(IEnumerable<K> saved) => io.Write(ToWrite(saved));
        public void Save(K saved) => Save(new[] { saved });
        public void Add(IEnumerable<K> saved) => io.Append("\n" + ToWrite(saved));
        public void Add(K saved) => Add(new[] { saved });
    }
}