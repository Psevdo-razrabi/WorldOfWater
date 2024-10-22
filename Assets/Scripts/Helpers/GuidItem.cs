using System;

namespace Helpers
{
    public struct GuidItem : IEquatable<GuidItem>
    {
        private uint _first;
        private uint _second;
        private uint _third;
        private uint _fourth;

        public GuidItem(Guid guid) : this()
        {
            _first = 0;
            _second = 0;
            _third = 0;
            _fourth = 0;
            ConvertGuidToFields(guid);
        }

        public GuidItem(uint first, uint second, uint third, uint fourth)
        {
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
        }

        public static GuidItem IsEmpty() => new GuidItem(0, 0, 0, 0);
        
        public bool Equals(GuidItem other)
        {
            return _first == other._first && _second == other._second && _third == other._third &&
                   _third == other._third;
        }

        public override bool Equals(object obj)
        {
            return obj is GuidItem guid && Equals(guid);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_first, _second, _third, _fourth);
        }

        public static implicit operator Guid(GuidItem guidItem) => guidItem.ToGuid();
        public static implicit operator GuidItem(Guid guid) => new (guid);
        public static Guid NewGuid() => Guid.NewGuid();

        public static bool operator ==(GuidItem first, GuidItem second) => first.Equals(second);

        public static bool operator !=(GuidItem first, GuidItem second) => !(first == second);
        
        public Guid ToGuid()
        {
            var bytes = new byte[16];
            BitConverter.GetBytes(_first).CopyTo(bytes, 0);
            BitConverter.GetBytes(_second).CopyTo(bytes, 4);
            BitConverter.GetBytes(_third).CopyTo(bytes, 8);
            BitConverter.GetBytes(_fourth).CopyTo(bytes, 12);

            return new Guid(bytes);
        }

        private void ConvertGuidToFields(Guid guid)
        {
            var bytes = guid.ToByteArray();
            _first = BitConverter.ToUInt32(bytes, 0);
            _second = BitConverter.ToUInt32(bytes, 4);
            _third = BitConverter.ToUInt32(bytes, 8);
            _fourth = BitConverter.ToUInt32(bytes, 12);
        }
    }
}