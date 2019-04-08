using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Funccy
{
    public static class LimiterExtensions
    {
        /// <summary>
        /// Rate limits a collection mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="arr"></param>
        /// <param name="limiter"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Task<U[]> Select<T, U>(
            this IEnumerable<T> arr,
            ILimiter limiter,
            Func<T, U> f
        ) => arr.Select(x => limiter.Run(() => f(x))).WhenAll();

        /// <summary>
        /// Rate limits a collection mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="arr"></param>
        /// <param name="limiter"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Task<U[]> Select<T, U>(
            this IEnumerable<T> arr,
            ILimiter limiter,
            Func<T, int, U> f
        ) => arr.Select((x, i) => limiter.Run(() => f(x, i))).WhenAll();

        /// <summary>
        /// Rate limits a collection mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="arr"></param>
        /// <param name="limiter"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Task<U[]> Select<T, U>(
            this IEnumerable<T> arr,
            ILimiter limiter,
            Func<T, Task<U>> f
        ) => arr.Select(x => limiter.Run(() => f(x))).WhenAll();

        /// <summary>
        /// Rate limits a collection mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="arr"></param>
        /// <param name="limiter"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Task<U[]> Select<T, U>(
            this IEnumerable<T> arr,
            ILimiter limiter,
            Func<T, int, Task<U>> f
        ) => arr.Select((x, i) => limiter.Run(() => f(x, i))).WhenAll();
    }
}
