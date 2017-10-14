using System;
using System.Collections.Immutable;

namespace Funccy
{
    /// <summary>
    /// A type with an associated log.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Log<T, TLog>
    {
        private IImmutableList<TLog> _log = ImmutableList<TLog>.Empty;
        private readonly T _value;

        /// <summary>
        /// Creates a new Log.
        /// </summary>
        /// <param name="value"></param>
        public Log(T value)
        {
            _value = value;
        }

        /// <summary>
        /// Creates a new Log with history.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="log"></param>
        public Log(T value, IImmutableList<TLog> log)
        {
            _value = value;
            _log = log;
        }
        
        /// <summary>
        /// Maps a function onto the Log workflow.
        /// </summary>
        /// <typeparam name="TNext"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public Log<TNext, TLog> Map<TNext>(Func<T, TNext> f)
        {
            return new Log<TNext, TLog>(f(_value), _log);
        }

        /// <summary>
        /// Adds a log message.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public Log<T, TLog> Add(Func<T, TLog> f)
        {
            var next = _log.Add(f(_value));
            return new Log<T, TLog>(_value, next);
        }

        /// <summary>
        /// Gets the value and the log.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public (T, IImmutableList<TLog>) Extract()
        {
            return (_value, _log);
        }
    }
}
