using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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

        [Fact]
        public void OneOf_Serializes_Objects()
        {
            var converter = new OneOfConverter();

            var expectedFoo = "{\"F\":123}";
            var foo = new OneOf<Foo, Bar>(new Foo { F = 123 });
            var actualFoo = JsonConvert.SerializeObject(foo, Formatting.None, converter);

            var expectedBar = "{\"B\":456}";
            var bar = new OneOf<Foo, Bar>(new Bar { B = 456 });
            var actualBar = JsonConvert.SerializeObject(bar, Formatting.None, converter);

            var readFoo = JsonConvert.DeserializeObject<OneOf<Foo, Bar>>("{\"F\":333}", converter);
            var readBar = JsonConvert.DeserializeObject<OneOf<Foo, Bar>>("{\"B\":999}", converter);

            Assert.Equal(expectedFoo, actualFoo);
            Assert.Equal(expectedBar, actualBar);
            Assert.Equal(333, readFoo.Extract(f => f.F, b => -1));
            Assert.Equal(999, readBar.Extract(f => -1, b => b.B));
        }

        public class Foo { public int F { get; set; } }
        public class Bar { public int B { get; set; } }

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
