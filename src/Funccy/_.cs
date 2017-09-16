using System;
using System.Collections.Generic;

namespace Funccy
{
    /// <summary>
    /// Functional helpers:
    /// - Explicit typing for Funcs
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

        /// <summary>
        /// Explicitly types a Func of one arg.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Func<T1, T2> F<T1, T2>(Func<T1, T2> f)
        {
            return f;
        }

        /// <summary>
        /// Explicitly types a Func of two args.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Func<T1, T2, T3> F<T1, T2, T3>(Func<T1, T2, T3> f)
        {
            return f;
        }
    }
}
