using System;
using System.Threading.Tasks;

namespace Funccy
{
    public static class OneOfMapExtensions
    {
        /// <summary>
        /// Maps a function onto the OneOf workflow.
        /// </summary>
        /// <typeparam name="TANext"></typeparam>
        /// <typeparam name="TBNext"></typeparam>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static OneOf<TANext, TBNext> Map<TA, TB, TANext, TBNext>(
            this OneOf<TA, TB> o,
            Func<TA, TANext> f,
            Func<TB, TBNext> g
            )
        {
            return o.Bind(
                a => new OneOf<TANext, TBNext>(f(a)),
                b => new OneOf<TANext, TBNext>(g(b))
            );
        }

        /// <summary>
        /// Maps a function onto the OneOf workflow.
        /// </summary>
        /// <typeparam name="TANext"></typeparam>
        /// <typeparam name="TBNext"></typeparam>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static OneOf<TANext, TBNext, TCNext> Map<TA, TB, TC, TANext, TBNext, TCNext>(
            this OneOf<TA, TB, TC> o,
            Func<TA, TANext> f,
            Func<TB, TBNext> g,
            Func<TC, TCNext> h
            )
        {
            return o.Bind(
                a => new OneOf<TANext, TBNext, TCNext>(f(a)),
                b => new OneOf<TANext, TBNext, TCNext>(g(b)),
                c => new OneOf<TANext, TBNext, TCNext>(h(c))
            );
        }

        /// <summary>
        /// Maps a function onto the OneOf workflow.
        /// </summary>
        /// <typeparam name="TANext"></typeparam>
        /// <typeparam name="TBNext"></typeparam>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static OneOf<TANext, TBNext, TCNext, TDNext> Map<TA, TB, TC, TD, TANext, TBNext, TCNext, TDNext>(
            this OneOf<TA, TB, TC, TD> o,
            Func<TA, TANext> f,
            Func<TB, TBNext> g,
            Func<TC, TCNext> h,
            Func<TD, TDNext> i
            )
        {
            return o.Bind(
                a => new OneOf<TANext, TBNext, TCNext, TDNext>(f(a)),
                b => new OneOf<TANext, TBNext, TCNext, TDNext>(g(b)),
                c => new OneOf<TANext, TBNext, TCNext, TDNext>(h(c)),
                d => new OneOf<TANext, TBNext, TCNext, TDNext>(i(d))
            );
        }

        /// <summary>
        /// Maps a function onto the OneOf workflow when the value is an A type.
        /// </summary>
        /// <typeparam name="TANext"></typeparam>
        /// <typeparam name="TBNext"></typeparam>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static OneOf<TANext, TB> MapA<TA, TB, TANext>(
            this OneOf<TA, TB> o,
            Func<TA, TANext> f
            )
        {
            return o.Map(
                a => f(a),
                b => b
            );
        }

        /// <summary>
        /// Maps a function onto the OneOf workflow when the value is a B type.
        /// </summary>
        /// <typeparam name="TANext"></typeparam>
        /// <typeparam name="TBNext"></typeparam>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static OneOf<TA, TBNext> MapB<TA, TB, TBNext>(   
            this OneOf<TA, TB> o,
            Func<TB, TBNext> g
            )
        {
            return o.Map(
                a => a,
                b => g(b)
            );
        }

        /// <summary>
        /// Maps a function onto the OneOf workflow when it is available.
        /// </summary>
        /// <typeparam name="TANext"></typeparam>
        /// <typeparam name="TBNext"></typeparam>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static async Task<OneOf<TANext, TBNext>> Map<TA, TB, TANext, TBNext>(
            this Task<OneOf<TA, TB>> t,
            Func<TA, TANext> f,
            Func<TB, TBNext> g
            )
        {
            return (await t).Bind(
                a => new OneOf<TANext, TBNext>(f(a)),
                b => new OneOf<TANext, TBNext>(g(b))
            );
        }
    }
}
