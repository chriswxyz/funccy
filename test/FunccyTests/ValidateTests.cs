using Funccy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace FunccyTests
{
    [TestClass]
    public class ValidateTests
    {
        [TestMethod]
        public async Task Validate_Test()
        {
            var validator = new Validate<string>()
                .WithRule("noXs",
                    x => !x.Any(c => c == 'x'),
                    x => $"Has an x at position {x.IndexOf('x')}")
                .WithRule("under20",
                    x => x.Length < 20,
                    x => $"Length was {x.Length}")
                .WithRule("remoteValidation",
                    x => CheckWithServer(x),
                    x => "Remote service did not accept")
                ;

            var values = new[] {
                "hello world",
                "helloxx worldxx",
                "it's your world, you can put whatever you want in it"
            };

            var results = (await validator
                .CheckAll(values))
                .Select(GetValueOrProblemSummary)
                .ToArray();

            Assert.AreEqual("hello world", results[0]);
            Assert.AreEqual("Has an x at position 5", results[1]);
            Assert.AreEqual("Length was 52, Remote service did not accept", results[2]);
        }

        public static Task<bool> CheckWithServer(string input)
        {
            // pretend to check something remotely
            return Task.FromResult(!input.Contains("whatever"));
        }

        public static string GetValueOrProblemSummary(OneOf<string, Problem[]> result)
        {
            return result.Extract(
                    ok => ok,
                    ps => ps.Select(x => x.Description).StringJoin(", ")
                );
        }
    }
}
