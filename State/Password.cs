using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Imperit.State
{
	public readonly struct Password : IEquatable<Password>
	{
		static readonly SHA256 sha = SHA256.Create();
		readonly byte[] Hash;
		public Password(byte[] hash) => Hash = hash;
		public Password(string str) => Hash = sha.ComputeHash(Encoding.UTF8.GetBytes(str));
		public static Password Parse(string b64) => new Password(Convert.FromBase64String(b64));
		public bool Equals(Password other) => Hash.SequenceEqual(other.Hash);
		public bool IsCorrect(string other) => other is string && Equals(new Password(other));
		public override bool Equals(object? obj) => obj is Password pw && Equals(pw);
		public static bool operator ==(Password x, Password y) => x.Equals(y);
		public static bool operator !=(Password x, Password y) => !x.Equals(y);
		public override int GetHashCode() => Hash.GetHashCode();
		public override string ToString() => Convert.ToBase64String(Hash);
	}
}