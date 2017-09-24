using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Funccy
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// A task that resolves when all tasks in the collection resolve.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> coll)
        {
            return await Task.WhenAll(coll);
        }

        /// <summary>
        /// All elements of the collection except for the head.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static IEnumerable<T> Rest<T>(this IEnumerable<T> coll)
        {
            return coll.Skip(1);
        }

        /// <summary>
        /// Concatenates the members of a collection, using the specified
        /// separator between each member.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string StringJoin<T>(this IEnumerable<T> coll, string separator)
        {
            return string.Join(separator, coll);
        }

        public static void ForEach<T>(this IEnumerable<T> coll, Action<T> a)
        {
            foreach(var i in coll)
            {
                a(i);
            }
        }
    }
}
