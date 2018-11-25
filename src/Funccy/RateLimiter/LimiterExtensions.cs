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
        /// <param name="f"></param>
        /// <param name="limiter"></param>
        /// <returns></returns>
        public static async Task<U[]> Select<T, U>(
            this IEnumerable<T> arr,
            Func<T, U> f,
            ILimiter limiter)
        {
            var tasks = arr.Select(x => limiter.Run(() => f(x)));

            var results = await Task.WhenAll(tasks);

            return results;
        }

        /// <summary>
        /// Rate limits a collection mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="arr"></param>
        /// <param name="f"></param>
        /// <param name="limiter"></param>
        /// <returns></returns>
        public static async Task<U[]> Select<T, U>(
            this IEnumerable<T> arr,
            Func<T, Task<U>> f,
            ILimiter limiter)
        {
            var tasks = arr.Select(x => limiter.Run(() => f(x)));

            var results = await Task.WhenAll(tasks);

            return results;
        }
    }
}
