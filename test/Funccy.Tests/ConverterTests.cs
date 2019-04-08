using Newtonsoft.Json;
using Xunit;

namespace Funccy.Tests
{
    public class ConverterTests
    {
        [Fact]
        public void Complex()
        {
            var a = new MyComplexResult
            {
                Foo = new Maybe<MyFoo>(),
                Other = new OneOf<MyBar, MyBaz>(new MyBaz { Baz = 42 })
            };

            var converters = new JsonConverter[] { new OneOfConverter(), new MaybeConverter() };

            var expected = "{\"Foo\":null,\"Other\":{\"Kind\":\"MyBaz\",\"Baz\":42}}";
            var actual = JsonConvert.SerializeObject(a, Formatting.None, converters);

            var actualRead = JsonConvert.DeserializeObject<MyComplexResult>(expected, converters);

            Assert.Equal(expected, actual);
            Assert.False(actualRead.Foo.Map(x => true).Extract(false));
            Assert.Equal(42, actualRead.Other.Extract(bar => -1, baz => baz.Baz));
        }

        public class MyComplexResult
        {
            public Maybe<MyFoo> Foo { get; set; }
            public OneOf<MyBar, MyBaz> Other { get; set; }
        }

        public class MyFoo
        {

        }

        public class MyBar
        {
            public readonly string Kind = nameof(MyBar);
            public string Bar { get; set; }
        }

        public class MyBaz
        {
            public readonly string Kind = nameof(MyBaz);
            public int Baz { get; set; }
        }
    }
}
