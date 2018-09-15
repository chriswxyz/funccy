using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Funccy
{
    /// <summary>
    /// Limits the amount of actions run in a certain time span.
    /// </summary>
    public class RateLimiter
    {
        // inspired by https://codereview.stackexchange.com/questions/87132/throttle-actions-by-number-per-period
        private readonly TimeSpan _timeSpan;

        // Some facts about SemaphoreSlim:
        // 1. The constructor signature means (available resources, max resources)
        // 2. They're IDisposable, but only the AvailableWaitHandle prop
        // uses unmanaged resources, so there's no need to dispose the ones here.
        // See https://stackoverflow.com/questions/32033416/do-i-need-to-dispose-a-semaphoreslim/39195920#39195920
        // 3. They're runtime level, not OS level.
        private readonly SemaphoreSlim _actions;
        private readonly SemaphoreSlim _time;

        public RateLimiter(int actions, TimeSpan timeSpan)
        {
            _actions = new SemaphoreSlim(actions, actions);
            _time = new SemaphoreSlim(actions, actions);
            _timeSpan = timeSpan;
        }

        public async Task<T> Run<T>(Func<T> action)
        {
            return await Run(action, CancellationToken.None);
        }

        public async Task<T> Run<T>(Func<T> action, CancellationToken cancel)
        {
            await _actions.WaitAsync(cancel);

            try
            {
                await _time.WaitAsync(cancel);

                await Task.Delay(_timeSpan);

                _time.Release(1);

                return action();
            }
            finally
            {
                _actions.Release(1);
            }
        }

        public async Task<T> Run<T>(Func<Task<T>> action)
        {
            return await Run(action, CancellationToken.None);
        }

        public async Task<T> Run<T>(Func<Task<T>> action, CancellationToken cancel)
        {
            await _actions.WaitAsync(cancel);

            try
            {
                await _time.WaitAsync(cancel);

                await Task.Delay(_timeSpan);

                _time.Release(1);

                return await action();
            }
            finally
            {
                _actions.Release(1);
            }
        }
    }

    public static class LimiterExtensions
    {
        /// <summary>
        /// Rate limits a collection mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="arr"></param>
        /// <param name="f"></param>
        /// <param name="limiter"></param>
        /// <returns></returns>
        public static async Task<U[]> Select<T, U>(
            this IEnumerable<T> arr,
            Func<T, U> f,
            RateLimiter limiter)
        {
            var tasks = arr.Select(x => limiter.Run(() => f(x)));

            var results = await Task.WhenAll(tasks);

            return results;
        }

        /// <summary>
        /// Rate limits a collection mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="arr"></param>
        /// <param name="f"></param>
        /// <param name="limiter"></param>
        /// <returns></returns>
        public static async Task<U[]> Select<T, U>(
            this IEnumerable<T> arr,
            Func<T, Task<U>> f,
            RateLimiter limiter)
        {
            var tasks = arr.Select(x => limiter.Run(() => f(x)));

            var results = await Task.WhenAll(tasks);

            return results;
        }
    }
}
