using System;
using Helpers.Extensions;

namespace Helpers.PoolObject
{
    [Serializable]
    public readonly struct PoolKey : IEquatable<PoolKey>
    {
        private readonly string _name;
        private readonly int _hashedKey;

        public PoolKey(string name)
        {
            _name = name;
            _hashedKey = _name.ComputeHash();
        }

        public bool Equals(PoolKey other) => _hashedKey == other._hashedKey;
        
        public override bool Equals(object obj) => obj is PoolKey other && Equals(other);
        
        public override int GetHashCode() => _hashedKey;

        public override string ToString() => $"Key is: {_name}";

        public static bool operator ==(PoolKey first, PoolKey second) =>
            first._hashedKey == second._hashedKey;
        public static bool operator !=(PoolKey first, PoolKey second) =>
            first._hashedKey != second._hashedKey;
    }
}