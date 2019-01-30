using Funccy;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Funccy.Tests
{
    public class ValidateTests
    {
        [Fact]
        public async Task Validate_Test()
        {
            var beerService = new BeerService();
            var validator = new Validate<DispenseBeerCommand, BeerValidation>()
                .Must(
                    x => beerService.Exists(x.BeerId),
                    x => new BeerValidation(404, $"Beer {x.BeerId} not found"))
                .Must(
                    x => x.Kind.IsIn("glass", "growler"),
                    x => new BeerValidation(409, $"Unknown vessel: {x.Kind}"))
                .When(
                    x => x.Kind == "glass",
                    v => v.Must(
                        x => x.SizeInOz,
                        x => x > 0 && x <= 20,
                        (x, r) => new BeerValidation(419, $"Select a pour less than 20 oz"))
                )
                ;

            var commands = new[]
            {
                new DispenseBeerCommand(1, "glass", 16),
                new DispenseBeerCommand(8, "glass", 12),
                new DispenseBeerCommand(3, "glass", 64),
                new DispenseBeerCommand(3, "growler", 64),
            };

            var results = (await validator.CheckAll(commands))
                .Select(r => r.Extract(
                    ok => $"Drink up! {ok.BeerId}",
                    ng => ng.Select(GetProblemSummary).StringJoin(", ")))
                .ToArray();

            Assert.Equal("Drink up! 1", results[0]);
            Assert.Equal("(404) Beer 8 not found", results[1]);
            Assert.Equal("(419) Select a pour less than 20 oz", results[2]);
            Assert.Equal("Drink up! 3", results[3]);
        }

        public class DispenseBeerCommand
        {
            public DispenseBeerCommand(int beerId, string kind, decimal sizeInOz)
            {
                BeerId = beerId;
                Kind = kind;
                SizeInOz = sizeInOz;
            }

            public int BeerId { get; set; }
            public string Kind { get; }
            public decimal SizeInOz { get; set; }
        }

        public class BeerService
        {
            public Task<bool> Exists(int beerId)
            {
                // pretend to get from a DB
                return beerId
                    .IsIn(1, 2, 3, 4, 5)
                    .Defer();
            }
        }

        public class BeerValidation
        {
            public BeerValidation(int code, string moreInfo)
            {
                Code = code;
                MoreInfo = moreInfo;
            }

            public int Code { get; set; }
            public string MoreInfo { get; set; }
        }

        public static string GetProblemSummary(BeerValidation x)
        {
            return $"({x.Code}) {x.MoreInfo}";
        }
    }
}
