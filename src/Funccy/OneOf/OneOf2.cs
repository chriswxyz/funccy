using System;
using System.Collections.Generic;

namespace Funccy
{
    /// <summary>
    /// A type that is one of several types.
    /// </summary>
    /// <typeparam name="TA"></typeparam>
    /// <typeparam name="TB"></typeparam>
    public sealed class OneOf<TA, TB> : ValueObject<OneOf<TA, TB>>
    {
        private readonly char _tag;
        private readonly TA _a;
        private readonly TB _b;

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
        /// Binds a function onto the OneOf workflow.
        /// </summary>
        /// <typeparam name="TANext"></typeparam>
        /// <typeparam name="TBNext"></typeparam>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public OneOf<TANext, TBNext> Bind<TANext, TBNext>(
            Func<TA, OneOf<TANext, TBNext>> f,
            Func<TB, OneOf<TANext, TBNext>> g
            )
        {
            switch (_tag)
            {
                case 'a': return f(_a);
                case 'b': return g(_b);
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
            Func<TB, TResult> g)
        {
            switch (_tag)
            {
                case 'a': return f(_a);
                case 'b': return g(_b);
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
        }
    }
}
