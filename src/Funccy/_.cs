using System.Collections.Generic;

namespace Funccy
{
    /// <summary>
    /// Functional helpers:
    /// - Empty values
    /// </summary>
    public static class _
    {
        /// <summary>
        /// The identity function. Returns the input.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T Identity<T>(T t)
        {
            return t;
        }

        /// <summary>
        /// An empty enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Empty<T>()
        {
            return new T[0];
        }
    }
}
