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
            var validator = new Validate<string, string>()
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

        [TestMethod]
        public async Task Validate_CustomModels()
        {

            var validator = new Validate<string, ExampleErrorModel>()
                .WithRule("noXs",
                    x => !x.Any(c => c == 'x'),
                    x => new ExampleErrorModel(100, $"Has an x at position {x.IndexOf('x')}"))
                .WithRule("under20",
                    x => x.Length < 20,
                    x => new ExampleErrorModel(102, $"Length was {x.Length}"))
                .WithRule("remoteValidation",
                    x => CheckWithServer(x),
                    x => new ExampleErrorModel(107, "Remote service did not accept"))
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
            Assert.AreEqual("(100) Has an x at position 5", results[1]);
            Assert.AreEqual("(102) Length was 52, (107) Remote service did not accept", results[2]);
        }

        public static Task<bool> CheckWithServer(string input)
        {
            // pretend to check something remotely
            return Task.FromResult(!input.Contains("whatever"));
        }

        public static string GetValueOrProblemSummary(OneOf<string, Problem<string>[]> result)
        {
            return result.Extract(
                    ok => ok,
                    ps => ps.Select(x => x.Description).StringJoin(", ")
                );
        }

        public static string GetValueOrProblemSummary(OneOf<string, Problem<ExampleErrorModel>[]> result)
        {
            return result.Extract(
                    ok => ok,
                    ps => ps
                        .Select(x => x.Description)
                        .Select(x => $"({x.Code}) {x.MoreInfo}")
                        .StringJoin(", ")
                );
        }
    }
}
