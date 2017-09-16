using System;
using System.Collections.Generic;
using System.Linq;

namespace Funccy
{
    public static class MaybeCollectionExtensions
    {
        /// <summary>
        /// Gets a Maybe of the first element of a collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static Maybe<T> FirstMaybe<T>(this IEnumerable<T> coll)
        {
            var first = coll.FirstOrDefault();
            return first != null
                ? new Maybe<T>(first)
                : new Maybe<T>();
        }

        /// <summary>
        /// Gets a Maybe of the first element of a collection that matches a predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static Maybe<T> FirstMaybe<T>(this IEnumerable<T> coll, Func<T, bool> f)
        {
            var first = coll.FirstOrDefault(f);
            return first != null
                ? new Maybe<T>(first)
                : new Maybe<T>();
        }

        /// <summary>
        /// Gets a Maybe of the single element of a collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static Maybe<T> SingleMaybe<T>(this IEnumerable<T> coll)
        {
            var single = coll.SingleOrDefault();
            return single != null
                ? new Maybe<T>(single)
                : new Maybe<T>();
        }

        /// <summary>
        /// Gets a Maybe of the single element of a collection that matches a predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static Maybe<T> SingleMaybe<T>(this IEnumerable<T> coll, Func<T, bool> f)
        {
            var single = coll.SingleOrDefault(f);
            return single != null
                ? new Maybe<T>(single)
                : new Maybe<T>();
        }

        /// <summary>
        /// Filters a collection of Maybes where a value exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereJust<T>(this IEnumerable<Maybe<T>> coll)
        {
            return coll
                .Where(x => x.Map(v => true).Extract(false))
                .Select(x => x.Extract(() => throw new InvalidOperationException()));
        }

    }
}
