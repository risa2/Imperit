using System;
using System.Collections.Generic;

namespace Imperit.Services
{
	public interface ILoginSession
	{
		string Add(int user);
		void Update(string id, int user);
		void Remove(string id);
		int? Get(string id);
		void Clear();
	}
	public class LoginSession : ILoginSession
	{
		readonly Random rand = new Random();
		readonly Dictionary<string, int> dict = new Dictionary<string, int>();

		public string Add(int user)
		{
			var key = rand.NextId(20);
			while (dict.ContainsKey(key))
			{
				key = rand.NextId(20);
			}
			dict[key] = user;
			return key;
		}

		public int? Get(string id)
		{
			bool contains = dict.TryGetValue(id, out int value);
			return contains ? (int?)value : null;
		}
		public void Remove(string id) => dict.Remove(id);
		public void Update(string id, int user) => dict[id] = user;
		public void Clear() => dict.Clear();
	}
}
