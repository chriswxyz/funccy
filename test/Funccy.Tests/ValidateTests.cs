using Funccy;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Funccy.Tests
{
    using DispenseBeerValidator = Validate<DispenseBeerCommand, BeerValidation, BeerService, Beer>;

    public class ValidateTests
    {
        public static readonly DispenseBeerValidator DispenseBeerValidator =
            new DispenseBeerValidator(async (model, context, report) =>
            {
                var kind = model.Kind;
                var beerId = model.BeerId;
                var size = model.SizeInOz;

                if (!kind.IsIn("glass", "growler"))
                {
                    report(new BeerValidation(409, $"Unknown vessel: {kind}"));
                }

                if (kind == "glass" && size > 20)
                {
                    report(new BeerValidation(419, $"Select a pour less than 20 oz"));
                }

                if (kind == "growler" && size > 64)
                {
                    report(new BeerValidation(419, $"Select a pour less than 64 oz"));
                }

                var beer = await context.GetById(beerId);

                if (beer is null)
                {
                    return report(new BeerValidation(404, $"Beer {beerId} not found"));
                }

                return beer;
            });

        [Fact]
        public async Task Validate_Test()
        {
            var beerService = new BeerService();

            var commands = new[]
            {
                new DispenseBeerCommand(1, "glass", 16),
                new DispenseBeerCommand(8, "glass", 12),
                new DispenseBeerCommand(3, "glass", 64),
                new DispenseBeerCommand(3, "growler", 64),
                new DispenseBeerCommand(9, "zxcvb", 0)
            };

            var validations = await commands
                .Select(cmd => DispenseBeerValidator.Check(cmd, beerService))
                .WhenAll();

            var results = validations
                .ExtractAll(
                    ok => $"Drink up! {ok.Name}",
                    ng => ng.Select(GetProblemSummary).StringJoin(", ")
                )
                .ToArray();

            Assert.Equal("Drink up! Beer 1", results[0]);
            Assert.Equal("(404) Beer 8 not found", results[1]);
            Assert.Equal("(419) Select a pour less than 20 oz", results[2]);
            Assert.Equal("Drink up! Beer 3", results[3]);
            Assert.Equal("(409) Unknown vessel: zxcvb, (404) Beer 9 not found", results[4]);
        }

        public static string GetProblemSummary(BeerValidation x) => $"({x.Code}) {x.MoreInfo}";
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
        public Task<Beer> GetById(int beerId)
        {
            // pretend to get from a DB
            return beerId.IsIn(1, 2, 3, 4, 5)
                ? new Beer { Name = $"Beer {beerId}" }.Defer()
                : (null as Beer).Defer();
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

    public class Beer
    {
        public string Name { get; set; }
    }
}
