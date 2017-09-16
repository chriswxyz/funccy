using System;
using System.Collections.Generic;

namespace Funccy
{
    /// <summary>
    /// A type that may not have a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Maybe<T> : ValueObject<Maybe<T>>
    {
        private readonly T _value;
        private readonly bool _hasValue;

        /// <summary>
        /// Creates a maybe that has a value.
        /// </summary>
        /// <param name="value"></param>
        public Maybe(T value)
        {
            _value = value;
            _hasValue = true;
        }

        /// <summary>
        /// Creates a maybe that has no value.
        /// </summary>
        public Maybe()
        {
            _hasValue = false;
        }

        /// <summary>
        /// Binds a function onto the Maybe workflow.
        /// </summary>
        /// <typeparam name="TNext"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public Maybe<TNext> Bind<TNext>(Func<T, Maybe<TNext>> f)
        {
            return _hasValue
                ? f(_value)
                : new Maybe<TNext>();
        }
        
        /// <summary>
        /// Gets the value of the Maybe, or a default when there is no value.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T Extract(T defaultValue)
        {
            return _hasValue
                ? _value
                : defaultValue;
        }

        /// <summary>
        /// Components that determine value equality of Maybes.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _hasValue;
            yield return _value;
        }
    }

}
