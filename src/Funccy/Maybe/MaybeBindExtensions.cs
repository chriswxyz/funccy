using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Funccy
{
    public static class MaybeBindExtensions
    {
        /// <summary>
        /// Binds a function onto all Maybes in the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TNext"></typeparam>
        /// <param name="coll"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerable<Maybe<TNext>> BindAll<T, TNext>(
            this IEnumerable<Maybe<T>> coll,
            Func<T, Maybe<TNext>> f)
        {
            return coll.Select(x => x.Bind(f));
        }

        /// <summary>
        /// Binds a function onto all Maybes in the collection when they are available.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TNext"></typeparam>
        /// <param name="coll"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerable<Task<Maybe<TNext>>> BindAll<T, TNext>(
            this IEnumerable<Task<Maybe<T>>> coll,
            Func<T, Maybe<TNext>> f)
        {
            return coll.Select(x => x.Bind(f));
        }

        /// <summary>
        /// Binds a function onto the Maybe workflow, when it is available.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TNext"></typeparam>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static async Task<Maybe<TNext>> Bind<T, TNext>(
            this Task<Maybe<T>> m,
            Func<T, Maybe<TNext>> f)
        {
            return (await m).Bind(f);
        }
    }
}
