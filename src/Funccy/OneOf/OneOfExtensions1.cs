using System;
using System.Threading.Tasks;

namespace Funccy
{
    public static class OneOfExtensions
    {
        /// <summary>
        /// Maps a function after awaiting the previous result.
        /// </summary>
		public static OneOf<Task<TANext>, Task<TBNext>> Map<TA, TB, TANext, TBNext>(
            this OneOf<Task<TA>, Task<TB>> oneOf,
            Func<TA, TANext> fA,
            Func<TB, TBNext> fB)
        {
            return oneOf.Map(
                async x => fA(await x),
                async x => fB(await x)
            );
        }

        /// <summary>
        /// Extracts a value after awaiting the previous result.
        /// </summary>
		public static Task<TResult> Extract<TA, TB, TResult>(
            this OneOf<Task<TA>, Task<TB>> oneOf,
            Func<TA, TResult> fA,
            Func<TB, TResult> fB
                )
        {
            return oneOf.Extract(
                async x => fA(await x),
                async x => fB(await x)
            );
        }
    }
}
