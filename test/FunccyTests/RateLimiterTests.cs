using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Funccy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunccyTests
{
    [TestClass]
    public class RateLimiterTests
    {
        [TestMethod]
        public async Task RateLimiterLimitsRate()
        {
            var stopwatch = new Stopwatch();

            var limiter = new RateLimiter(3, TimeSpan.FromSeconds(1));

            stopwatch.Start();

            var nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var results = await nums.Select(
                x => stopwatch.ElapsedMilliseconds,
                limiter);
            
            stopwatch.Stop();

            Assert.IsTrue(results[0].IsNear(0, 100));
            Assert.IsTrue(results[1].IsNear(0, 100));
            Assert.IsTrue(results[2].IsNear(0, 100));

            Assert.IsTrue(results[3].IsNear(1000, 100));
            Assert.IsTrue(results[4].IsNear(1000, 100));
            Assert.IsTrue(results[5].IsNear(1000, 100));

            Assert.IsTrue(results[6].IsNear(2000, 100));
            Assert.IsTrue(results[7].IsNear(2000, 100));
            Assert.IsTrue(results[8].IsNear(2000, 100));
        }
    }
}
