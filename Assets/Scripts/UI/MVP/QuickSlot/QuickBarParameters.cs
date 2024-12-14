using System;

namespace QuickSlot
{
    [Serializable]
    public struct QuickBarParameters
    {
        public int capacity;

        public QuickBarParameters(int capacity)
        {
            this.capacity = capacity;
        }
    }
}