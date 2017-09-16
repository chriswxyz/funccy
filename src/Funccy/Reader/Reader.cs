using System;

namespace Funccy
{
    public class Reader<TState>
    {
        private TState _state;

        public Reader(TState state)
        {
            _state = state;
        }

        public Reader<TState, TNext> Bind<TNext>(Func<TState, Reader<TState, TNext>> f)
        {
            return f(_state);
        }

        public Reader<TState, TNext> Map<TNext>(Func<TState, TNext> f)
        {
            return Bind(x => new Reader<TState, TNext>(x, f(x)));
        }
    }

    public class Reader<TState, TValue>
    {
        private TState _state;
        private TValue _value;

        public Reader(TState state, TValue value)
        {
            _state = state;
            _value = value;
        }

        public Reader<TState, TNext> Bind<TNext>(Func<TState, TValue, Reader<TState, TNext>> f)
        {
            return f(_state, _value);
        }

        public Reader<TState, TNext> Map<TNext>(Func<TState, TValue, TNext> f)
        {
            return Bind((state, value) => new Reader<TState, TNext>(state, f(state, value)));
        }

        public TValue Unwrap()
        {
            return _value;
        }

        public TResult Unwrap<TResult>(Func<TState, TResult> f)
        {
            return f(_state);
        }
    }
}
