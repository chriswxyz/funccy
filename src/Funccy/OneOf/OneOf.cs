using System;
using System.Collections.Generic;

namespace Funccy
{
    /// <summary>
    /// A value that could be one of two types.
    /// </summary>
    public sealed class OneOf<TA, TB> : ValueObject<OneOf<TA, TB>>
    {
        private readonly char _tag;
        private readonly TA _A;
        private readonly TB _B;

        public OneOf(TA valueA)
        {
            _tag = 'A';
            _A = valueA;
        }

        public OneOf(TB valueB)
        {
            _tag = 'B';
            _B = valueB;
        }

        public OneOf<TANext, TBNext> Map<TANext, TBNext>(
            Func<TA, TANext> fA,
            Func<TB, TBNext> fB
        )
        {
            switch (_tag)
            {
                case 'A':
                    return new OneOf<TANext, TBNext>(fA(_A));
                case 'B':
                    return new OneOf<TANext, TBNext>(fB(_B));
                default:
                    throw new InvalidOperationException();
            }
        }

        public TResult Extract<TResult>(
            Func<TA, TResult> fA,
            Func<TB, TResult> fB
        )
        {
            switch (_tag)
            {
                case 'A':
                    return fA(_A);
                case 'B':
                    return fB(_B);
                default:
                    throw new InvalidOperationException();
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _tag;
            yield return _A;
            yield return _B;
        }
    }
}
