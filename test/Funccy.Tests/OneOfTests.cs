using System.Threading.Tasks;
using Xunit;

namespace Funccy.Tests
{
    public class OneOfTests
    {
        [Fact]
        public async Task OneOf_Test()
        {
            var oneOfA = new OneOf<string, int>("test");
            var oneOfB = new OneOf<string, int>(100);

            var resultA = await Process(oneOfA);
            var resultB = await Process(oneOfB);

            Assert.Equal("test!$$", resultA);
            Assert.Equal("404", resultB);
        }

        private static Task<string> Process(OneOf<string, int> input)
        {
            return input
                .Map(
                    x => Task.FromResult(x + "!"),
                    x => Task.FromResult(x + 1)
                )
                .Map(
                    x => x + "$$",
                    x => x * 4
                )
                .Extract(
                    a => a,
                    b => b.ToString()
                );
        }
    }
}
