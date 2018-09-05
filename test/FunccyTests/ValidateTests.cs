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
            var validator = new Validate<string, ExampleErrorModel>()
                .WithRule(
                    x => !x.Any(c => c == 'x'),
                    x => new ExampleErrorModel(100, $"Has an x at position {x.IndexOf('x')}"))
                .WithRule(
                    x => x.Length < 20,
                    x => new ExampleErrorModel(102, $"Length was {x.Length}"))
                .WithRule(
                    x => CheckWithServer(x),
                    x => new ExampleErrorModel(107, "Remote service did not accept"))
                .WithRule(
                    x => x.Contains("hello"),
                    x => !x.Contains("goodbye"),
                    x => new ExampleErrorModel(113, "Hello message cannot contain goodbyes"))
                ;

            var values = new[] {
                "hello world",
                "helloxx worldxx",
                "it's your world, you can put whatever you want in it",
                "hello goodbye"
            };

            var results = (await validator
                .CheckAll(values))
                .Select(GetValueOrProblemSummary)
                .ToArray();

            Assert.AreEqual("hello world", results[0]);
            Assert.AreEqual("(100) Has an x at position 5", results[1]);
            Assert.AreEqual("(102) Length was 52, (107) Remote service did not accept", results[2]);
            Assert.AreEqual("(113) Hello message cannot contain goodbyes", results[3]);
        }

        public class ExampleErrorModel
        {
            public ExampleErrorModel(int code, string moreInfo)
            {
                Code = code;
                MoreInfo = moreInfo;
            }

            public int Code { get; set; }
            public string MoreInfo { get; set; }
        }

        public static Task<bool> CheckWithServer(string input)
        {
            // pretend to check something remotely
            return Task.FromResult(!input.Contains("whatever"));
        }
        
        public static string GetValueOrProblemSummary(OneOf<string, ExampleErrorModel[]> result)
        {
            return result.Extract(
                    ok => ok,
                    ps => ps
                        .Select(x => $"({x.Code}) {x.MoreInfo}")
                        .StringJoin(", ")
                );
        }
    }
}
