using System;
using System.Collections.Generic;
using System.Linq;

namespace Funccy
{
    public static class OneOfExtensions2
    {
        /// <summary>
        /// Filters the elements of the collection where the value is type A.
        /// </summary>
        /// <typeparam name="TA"></typeparam>
        /// <typeparam name="TB"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static IEnumerable<TA> WhereA<TA, TB>(this IEnumerable<OneOf<TA, TB>> coll)
        {
            return coll
                .Where(x => x.Extract(a => true, b => false))
                .Select(x => x.Extract(a => a, b => throw new InvalidOperationException()));
        }

        /// <summary>
        /// Filters the elements of the collection where the value is type B.
        /// </summary>
        /// <typeparam name="TA"></typeparam>
        /// <typeparam name="TB"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static IEnumerable<TB> WhereB<TA, TB>(this IEnumerable<OneOf<TA, TB>> coll)
        {
            return coll
                .Where(x => x.Extract(a => false, b => true))
                .Select(x => x.Extract(a => throw new NotSupportedException(), b => b));

        }

        public static IEnumerable<TNext> ExtractAll<TA, TB, TNext>(
            this IEnumerable<OneOf<TA, TB>> oneOfs,
            Func<TA, TNext> a,
            Func<TB, TNext> b)
        {
            return oneOfs.Select(x => x.Extract(a, b));
        }

        /// <summary>
        /// Splits the elements of the collection based on type.
        /// </summary>
        /// <typeparam name="TA"></typeparam>
        /// <typeparam name="TB"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="coll"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static TResult Partition<TA, TB, TResult>(
            this IEnumerable<OneOf<TA, TB>> coll,
            Func<IEnumerable<TA>, IEnumerable<TB>, TResult> f)
        {
            var As = coll.WhereA();
            var Bs = coll.WhereB();
            return f(As, Bs);
        }
    }
}
