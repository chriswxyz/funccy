using Funccy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FunccyTests
{
    [TestClass]
    public class MaybeTests
    {

        [TestMethod]
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

            Assert.AreEqual("hello world!", results[0]);
            Assert.AreEqual("no value", results[1]);
        }

        [TestMethod]
        public void Maybe_ObeysRule_Identity()
        {
            var m = new Maybe<string>("hello");
            var m2 = m.Map(x => x);

            Assert.AreEqual(m, m2);
        }

        [TestMethod]
        public void Maybe_ObeysRule_Composition()
        {
            var m = new Maybe<string>("hello");

            string f(string x) => x + "f";
            string g(string x) => x + "g";

            var one = m.Map(x => f(g(x)));
            var two = m.Map(g).Map(f);

            Assert.AreEqual(one, two);
        }

        [TestMethod]
        public void Maybe_ObeysRule_LeftIdentity()
        {
            var str = "hello";
            var m = new Maybe<string>(str);

            Maybe<string> f(string x) => new Maybe<string>(x + "f");

            var one = m.Bind(f);
            var two = f(str);

            Assert.AreEqual(one, two);
        }

        [TestMethod]
        public void Maybe_ObeysRule_RightIdentity()
        {
            var m = new Maybe<string>("hello");
            var m2 = m.Bind(x => new Maybe<string>(x));

            Assert.AreEqual(m, m2);
        }
        
        bool HasValue<T>(Maybe<T> m) => m.Map(x => true).Extract(false);

        [TestMethod]
        public void Maybe_SingleMaybe_ReturnsNone()
        {
            var vals = new[] { 1, 2, 3 };

            var none1 = vals.SingleMaybe();
            var none2 = vals.SingleMaybe(x => x > 4);

            Assert.IsFalse(HasValue(none1));
            Assert.IsFalse(HasValue(none2));
        }

        [TestMethod]
        public void Maybe_SingleMaybe_ReturnsSome()
        {
            var vals1 = new[] { 1 };
            var some1 = vals1.SingleMaybe();

            var vals2 = new[] { 1, 2, 4 };
            var some2 = vals2.SingleMaybe(x => x > 3);

            Assert.IsTrue(HasValue(some1));
            Assert.IsTrue(HasValue(some2));
        }
    }
}
