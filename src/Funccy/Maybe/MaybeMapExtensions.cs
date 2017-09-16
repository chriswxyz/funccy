using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Funccy
{
    public static class MaybeMapExtensions
    {

        /// <summary>
        /// Maps a function onto the Maybe workflow.
        /// </summary>
        /// <typeparam name="TNext"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Maybe<TNext> Map<T, TNext>(this Maybe<T> m, Func<T, TNext> f)
        {
            return m.Bind(x => new Maybe<TNext>(f(x)));
        }

        /// <summary>
        /// Map a function onto the Maybe workflow, when it's available.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TNext"></typeparam>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static async Task<Maybe<TNext>> Map<T, TNext>(
            this Task<Maybe<T>> m,
            Func<T, TNext> f)
        {
            return (await m).Map(f);
        }

        /// <summary>
        /// Map a function onto all Maybes in the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TNext"></typeparam>
        /// <param name="coll"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerable<Maybe<TNext>> Map<T, TNext>(
            this IEnumerable<Maybe<T>> coll,
            Func<T, TNext> f)
        {
            return coll.Select(x => x.Map(f));
        }

        /// <summary>
        /// Map a function onto all Maybes in the collection when they are available.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TNext"></typeparam>
        /// <param name="coll"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerable<Task<Maybe<TNext>>> Map<T, TNext>(
            this IEnumerable<Task<Maybe<T>>> coll,
            Func<T, TNext> f)
        {
            return coll.Select(x => x.Map(f));
        }
    }
}
