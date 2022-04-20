using System.Collections.Generic;
using System.Linq;

namespace InTwitter.Extensions
{
    public static class IEnumerableExtension
    {
        public static bool IsAny<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }
    }
}
