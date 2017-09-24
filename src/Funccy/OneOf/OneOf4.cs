using System;
using System.Collections.Generic;

namespace Funccy
{
    /// <summary>
    /// A type that is one of several types.
    /// </summary>
    /// <typeparam name="TA"></typeparam>
    /// <typeparam name="TB"></typeparam>
    public class OneOf<TA, TB, TC, TD> : ValueObject<OneOf<TA, TB, TC, TD>>
    {
        private readonly char _tag;
        private readonly TA _a;
        private readonly TB _b;
        private readonly TC _c;
        private readonly TD _d;

        /// <summary>
        /// Creates a OneOf with the A value.
        /// </summary>
        /// <param name="value"></param>
        public OneOf(TA value)
        {
            _tag = 'a';
            _a = value;
        }

        /// <summary>
        /// Creates a OneOf with the B value.
        /// </summary>
        /// <param name="value"></param>
        public OneOf(TB value)
        {
            _tag = 'b';
            _b = value;
        }

        /// <summary>
        /// Creates a OneOf with the C value.
        /// </summary>
        /// <param name="value"></param>
        public OneOf(TC value)
        {
            _tag = 'c';
            _c = value;
        }

        /// <summary>
        /// Creates a OneOf with the D value.
        /// </summary>
        /// <param name="value"></param>
        public OneOf(TD value)
        {
            _tag = 'd';
            _d = value;
        }

        /// <summary>
        /// Binds a function onto the OneOf workflow.
        /// </summary>
        /// <typeparam name="TANext"></typeparam>
        /// <typeparam name="TBNext"></typeparam>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public OneOf<TANext, TBNext, TCNext, TDNext> Bind<TANext, TBNext, TCNext, TDNext>(
            Func<TA, OneOf<TANext, TBNext, TCNext, TDNext>> f,
            Func<TB, OneOf<TANext, TBNext, TCNext, TDNext>> g,
            Func<TC, OneOf<TANext, TBNext, TCNext, TDNext>> h,
            Func<TD, OneOf<TANext, TBNext, TCNext, TDNext>> i

            )
        {
            switch (_tag)
            {
                case 'a': return f(_a);
                case 'b': return g(_b);
                case 'c': return h(_c);
                case 'd': return i(_d);
                default: throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Matches the OneOf down to a result type.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public TResult Extract<TResult>(
            Func<TA, TResult> f,
            Func<TB, TResult> g,
            Func<TC, TResult> h,
            Func<TD, TResult> i
            )
        {
            switch (_tag)
            {
                case 'a': return f(_a);
                case 'b': return g(_b);
                case 'c': return h(_c);
                case 'd': return i(_d);
                default: throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Components that determine value equality of OneOfs.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _tag;
            yield return _a;
            yield return _b;
            yield return _c;
            yield return _d;
        }
    }
}
