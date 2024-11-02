using System;
using Helpers.Extensions;

namespace Sync
{
    [Serializable]
    public readonly struct ResourcesKey : IEquatable<ResourcesKey>
    {
        private readonly string _name;
        private readonly int _hashedKey;

        public ResourcesKey(string name)
        {
            _name = name;
            _hashedKey = _name.ComputeHash();
        }

        public bool Equals(ResourcesKey other) => _hashedKey == other._hashedKey;
        
        public override bool Equals(object obj) => obj is ResourcesKey other && Equals(other);
        
        public override int GetHashCode() => _hashedKey;

        public override string ToString() => $"Key is: {_name}";

        public static bool operator ==(ResourcesKey first, ResourcesKey second) =>
            first._hashedKey == second._hashedKey;
        public static bool operator !=(ResourcesKey first, ResourcesKey second) =>
            first._hashedKey != second._hashedKey;
    }
}