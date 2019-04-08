using Newtonsoft.Json;
using System.Linq;
using Xunit;

namespace Funccy.Tests
{
    public class MaybeTests
    {
        [Fact]
        public void Maybe_Test()
        {
            var values = new[]
            {
                new Maybe<string>("hello"),
                new Maybe<string>()
            };

            var results = values
                .Map(x => $"{x} world!")
                .Extract("no value")
                .ToArray()
                ;

            Assert.Equal("hello world!", results[0]);
            Assert.Equal("no value", results[1]);
        }

        [Fact]
        public void Maybe_ObeysRule_Identity()
        {
            var m = new Maybe<string>("hello");
            var m2 = m.Map(x => x);

            Assert.Equal(m, m2);
        }

        [Fact]
        public void Maybe_ObeysRule_Composition()
        {
            var m = new Maybe<string>("hello");

            string f(string x) => x + "f";
            string g(string x) => x + "g";

            var one = m.Map(x => f(g(x)));
            var two = m.Map(g).Map(f);

            Assert.Equal(one, two);
        }

        [Fact]
        public void Maybe_ObeysRule_LeftIdentity()
        {
            var str = "hello";
            var m = new Maybe<string>(str);

            Maybe<string> f(string x) => new Maybe<string>(x + "f");

            var one = m.Bind(f);
            var two = f(str);

            Assert.Equal(one, two);
        }

        [Fact]
        public void Maybe_ObeysRule_RightIdentity()
        {
            var m = new Maybe<string>("hello");
            var m2 = m.Bind(x => new Maybe<string>(x));

            Assert.Equal(m, m2);
        }

        bool HasValue<T>(Maybe<T> m) => m.Map(x => true).Extract(false);

        [Fact]
        public void Maybe_SingleMaybe_ReturnsNone()
        {
            var vals = new[] { 1, 2, 3 };

            var none1 = vals.SingleMaybe();
            var none2 = vals.SingleMaybe(x => x > 4);

            Assert.False(HasValue(none1));
            Assert.False(HasValue(none2));
        }

        [Fact]
        public void Maybe_SingleMaybe_ReturnsSome()
        {
            var vals1 = new[] { 1 };
            var some1 = vals1.SingleMaybe();

            var vals2 = new[] { 1, 2, 4 };
            var some2 = vals2.SingleMaybe(x => x > 3);

            Assert.True(HasValue(some1));
            Assert.True(HasValue(some2));
        }

        [Fact]
        public void Maybe_Serializes_Objects()
        {
            var converter = new MaybeConverter();

            var expectedJust = "{\"F\":123}";
            var just = new Maybe<Foo>(new Foo { F = 123 });
            var actualJust = JsonConvert.SerializeObject(just, Formatting.None, converter);

            var expectedNone = "null";
            var none = new Maybe<Foo>();
            var actualNone = JsonConvert.SerializeObject(none, Formatting.None, converter);

            var expectedInt = "789";
            var justInt = new Maybe<int>(789);
            var actualInt = JsonConvert.SerializeObject(justInt, Formatting.None, converter);

            var actualReadFoo = JsonConvert.DeserializeObject<Maybe<Foo>>("{\"F\":999}", converter);
            var actualReadNone = JsonConvert.DeserializeObject<Maybe<Foo>>("null", converter);
            var actualReadInt = JsonConvert.DeserializeObject<Maybe<int>>("456", converter);

            Assert.Equal(expectedJust, actualJust);
            Assert.Equal(expectedNone, actualNone);
            Assert.Equal(expectedInt, actualInt);
            Assert.Equal(-1, actualReadNone.Map(x => x.F).Extract(-1));
            Assert.Equal(999, actualReadFoo.Map(x => x.F).Extract(-1));
            Assert.Equal(456, actualReadInt.Extract(-1));
        }

        public class Foo { public int F { get; set; } }
    }
}
