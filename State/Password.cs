using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Imperit.State
{
    public readonly struct Password : IEquatable<Password>
    {
        static readonly SHA256 sha = SHA256.Create();
        public readonly byte[] Hash;
        public Password(byte[] hash) => Hash = hash;
        public static Password FromString(string str) => new Password(sha.ComputeHash(Encoding.UTF8.GetBytes(str)));
        public bool Equals(Password other) => Hash.SequenceEqual(other.Hash);
        public override bool Equals(object? obj) => obj is Password pw && Equals(pw);
        public static bool operator ==(Password x, Password y) => x.Equals(y);
        public static bool operator !=(Password x, Password y) => !x.Equals(y);
        public override int GetHashCode() => Hash.GetHashCode();
        public static Password Parse(string str) => new Password(Convert.FromBase64String(str));
        public override string ToString() => Convert.ToBase64String(Hash);
    }
}