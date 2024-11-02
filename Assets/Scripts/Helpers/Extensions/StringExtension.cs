using System.Linq;

namespace Helpers.Extensions
{
    public static class StringExtension
    {
        public static int ComputeHash(this string str)
        {
            return unchecked((int)str.Aggregate(2166136261, (hash, symbol) => (hash ^ symbol) * 16777619));
        }
    }
}