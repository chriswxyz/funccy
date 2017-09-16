using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Funccy
{
    public static class CollectionExtensions
    {
        public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> coll)
        {
            return await Task.WhenAll(coll);
        }

        public static async Task<T[]> ToArray<T>(this Task<IEnumerable<T>> coll)
        {
            return (await coll).ToArray();
        }
    }
}
