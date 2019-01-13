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

        /// <summary>
        /// Filters a collection with an async predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> WhereAsync<T>(this IEnumerable<T> coll, Func<T, Task<bool>> f)
        {
            async Task<(bool match, T item)> Check(T model) => (await f(model), model);

            var tasks = await coll.Select(Check).WhenAll();

            return tasks.Where(x => x.match).Select(x => x.item);
        }


        /// <summary>
        /// Returns true if there are no items in the sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static bool None<T>(this IEnumerable<T> arr)
        {
            return !arr.Any();
        }

        /// <summary>
        /// Returns true if none of the items in the sequence meet the condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool None<T>(this IEnumerable<T> arr, Func<T, bool> f)
        {
            return !arr.Any(f);
        }

        /// <summary>
        /// Returns all elements of a sequence that do not match the predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> arr, Func<T, bool> f)
        {
            return arr.Where(f.Not());
        }

        /// <summary>
        /// Returns all elements of a sequence that do not match the predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> WhereNotAsync<T>(this IEnumerable<T> arr, Func<T, Task<bool>> f)
        {
            return arr.WhereAsync(async x => !await f(x));
        }
    }
}
