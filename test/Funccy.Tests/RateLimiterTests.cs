﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace Funccy.Tests
{
    public class RateLimiterTests
    {
        [Fact]
        public async Task RateLimiterLimitsRate()
        {
            var stopwatch = new Stopwatch();

            var limiter = new RateLimiter(3, TimeSpan.FromSeconds(1));

            var nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            stopwatch.Start();

            var results = await nums.Select(
                x => stopwatch.ElapsedMilliseconds,
                limiter);

            stopwatch.Stop();

            Assert.True(results[0].IsNear(0, 100));
            Assert.True(results[1].IsNear(0, 100));
            Assert.True(results[2].IsNear(0, 100));

            Assert.True(results[3].IsNear(1000, 100));
            Assert.True(results[4].IsNear(1000, 100));
            Assert.True(results[5].IsNear(1000, 100));

            Assert.True(results[6].IsNear(2000, 100));
            Assert.True(results[7].IsNear(2000, 100));
            Assert.True(results[8].IsNear(2000, 100));
        }

        [Fact]
        public async Task ConcurrentLimiterLimitsConcurrency()
        {
            var stopwatch = new Stopwatch();

            var limiter = new ConcurrentLimiter(3);

            var nums = new[] { 1, 2, 3, 4 };

            stopwatch.Start();

            var results = await nums.Select(
                async x =>
                {
                    var time = stopwatch.ElapsedMilliseconds;
                    await Task.Delay(500);
                    return time;
                },
                limiter);

            stopwatch.Stop();

            Assert.True(results[0].IsNear(0, 100));
            Assert.True(results[1].IsNear(0, 100));
            Assert.True(results[2].IsNear(0, 100));

            Assert.True(results[3].IsNear(500, 100));
        }
    }
}
