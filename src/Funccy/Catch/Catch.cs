using System;

namespace Funccy
{
    /// <summary>
    /// A type that will catch a specific exception.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TException"></typeparam>
    public sealed class Catch<TValue, TException> where TException : Exception
    {
        private readonly TValue _value;
        private readonly TException _ex;
        private readonly bool _hasEx;

        /// <summary>
        /// Creates a new Catch for an object.
        /// </summary>
        /// <param name="value"></param>
        public Catch(TValue value)
        {
            _value = value;
            _hasEx = false;
        }

        public Catch(TException ex)
        {
            _ex = ex;
            _hasEx = true;
        }

        /// <summary>
        /// Tries an operation and returns the result or the caught exception.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public OneOf<TResult, TException> Extract<TResult>(Func<TValue, TResult> f)
        {
            try
            {
                return new OneOf<TResult, TException>(f(_value));
            }
            catch (TException e)
            {
                return new OneOf<TResult, TException>(e);
            }
        }
    }

    public class CatchBuilder<TValue>
    {
        private readonly TValue _value;

        public CatchBuilder(TValue value)
        {
            _value = value;
        }

        public Catch<TValue, TException> Handle<TException>() where TException : Exception
        {
            return new Catch<TValue, TException>(_value);
        }
    }
}
