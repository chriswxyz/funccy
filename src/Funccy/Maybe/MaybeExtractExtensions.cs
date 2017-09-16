using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Funccy
{
    public static class MaybeExtractExtensions
    {
        /// <summary>
        /// Extracts a Maybe using a func.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Extract<T>(this Maybe<T> m, Func<T> defaultValue)
        {
            var val = defaultValue();
            return m.Extract(val);
        }

        /// <summary>
        /// Extracts a value or default from all Maybes in the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static IEnumerable<T> Extract<T>(
            this IEnumerable<Maybe<T>> coll,
            T defaultValue)
        {
            return coll.Select(x => x.Extract(defaultValue));
        }

        /// <summary>
        /// Extracts a value or default from all Maybes in the collection, when they are available.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static IEnumerable<Task<T>> Extract<T>(
            this IEnumerable<Task<Maybe<T>>> coll,
            T defaultValue)
        {
            return coll.Select(x => x.Extract(defaultValue));
        }

        /// <summary>
        /// Extract the value or a default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static async Task<T> Extract<T>(
            this Task<Maybe<T>> m,
            T defaultValue)
        {
            return (await m).Extract(defaultValue);
        }

        /// <summary>
        /// Extract the value or a default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static async Task<T> Extract<T>(
            this Task<Maybe<T>> m,
            Func<T> defaultValue)
        {
            return (await m).Extract(defaultValue);
        }
    }
}
