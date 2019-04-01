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
        public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> coll) =>
            await Task.WhenAll(coll);

        /// <summary>
        /// All elements of the collection except for the head.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static IEnumerable<T> Rest<T>(this IEnumerable<T> coll) =>
            coll.Skip(1);

        /// <summary>
        /// Concatenates the members of a collection, using the specified
        /// separator between each member.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string StringJoin<T>(this IEnumerable<T> coll, string separator) =>
            string.Join(separator, coll);

        /// <summary>
        /// Filters a collection with an async predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> WhereAsync<T>(this IEnumerable<T> coll, Func<T, Task<bool>> f) =>
            (await coll
                .Select(async model => (ok: await f(model), model))
                .WhenAll())
                .Where(x => x.ok)
                .Select(x => x.model);

        /// <summary>
        /// Returns true if there are no items in the sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static bool None<T>(this IEnumerable<T> arr) =>
            !arr.Any();

        /// <summary>
        /// Returns true if none of the items in the sequence meet the condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool None<T>(this IEnumerable<T> arr, Func<T, bool> f) =>
            !arr.Any(f);

        /// <summary>
        /// Returns all elements of a sequence that do not match the predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> arr, Func<T, bool> f) =>
            arr.Where(f.Not());

        /// <summary>
        /// Returns all elements of a sequence that do not match the predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> WhereNotAsync<T>(this IEnumerable<T> arr, Func<T, Task<bool>> f) =>
            arr.WhereAsync(async x => !await f(x));

        /// <summary>
        /// Projects each element of a sequence into a new array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TNext"></typeparam>
        /// <param name="arr"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static TNext[] SelectArray<T, TNext>(this IEnumerable<T> arr, Func<T, TNext> f) =>
            arr.Select(f).ToArray();
    }
}
