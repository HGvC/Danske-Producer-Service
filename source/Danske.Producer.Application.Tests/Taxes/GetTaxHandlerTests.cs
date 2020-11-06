using Danske.Producer.Application.Taxes.Queries;
using Danske.Producer.Domain.Tax;
using Danske.Producer.Infrastructure;
using FluentAssertions;
using LanguageExt.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Danske.Producer.Application.Tests.Taxes
{
    [UnitTest]
    public class GetTaxHandlerTests
    {
        private readonly GetTaxHandler _handler;
        private readonly TaxesDbContext _dbContext;

        public GetTaxHandlerTests()
        {
            var options = new DbContextOptionsBuilder<TaxesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new TaxesDbContext(options);
            _handler = new GetTaxHandler(_dbContext);
        }

        [Fact]
        public void CreateHandler_GivenNullTaxesDbContext_ThrowsArgumentNullException()
        {
            Action action = () => new GetTaxHandler(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Handle_GivenTaxWasFound_ReturnsProjectAsync()
        {
            var vilnius = new Tax
            {
                Municipality = "Vilnius",
                PeriodType = Enums.PeriodType.Monthly,
                PeriodStart = DateTime.Parse("2020-11-08"),
                PeriodEnd = DateTime.Parse("2020-11-08"),
                Result = 0.2m
            };
            var varena = new Tax
            {
                Municipality = "Varena",
                PeriodType = Enums.PeriodType.Monthly,
                PeriodStart = DateTime.Parse("2020-11-08"),
                PeriodEnd = DateTime.Parse("2020-11-08"),
                Result = 0.2m
            };

            await _dbContext.AddRangeAsync(vilnius, varena);
            await _dbContext.SaveChangesAsync();

            var request = new GetTax("Vilnius", "2020-11-08");

            var result = await _handler.Handle(request, default);

            result.ShouldBeSome(project =>
            {
                var expectation = new Tax
                {
                    Municipality = "Vilnius",
                    PeriodType = Enums.PeriodType.Monthly,
                    PeriodStart = DateTime.Parse("2020-11-08"),
                    PeriodEnd = DateTime.Parse("2020-11-08"),
                    Result = 0.2m
                };

                project.Should().BeEquivalentTo(expectation);
            });
        }

        [Fact]
        public async Task Handle_GivenTaxWasNotFound_ReturnsNoneAsync()
        {
            var vilnius = new Tax
            {
                Municipality = "Vilnius",
                PeriodType = Enums.PeriodType.Monthly,
                PeriodStart = DateTime.Parse("2020-11-08"),
                PeriodEnd = DateTime.Parse("2020-11-08"),
                Result = 0.2m
            };
            var varena = new Tax
            {
                Municipality = "Varena",
                PeriodType = Enums.PeriodType.Monthly,
                PeriodStart = DateTime.Parse("2020-11-08"),
                PeriodEnd = DateTime.Parse("2020-11-08"),
                Result = 0.2m
            };

            await _dbContext.AddRangeAsync(vilnius, varena);
            await _dbContext.SaveChangesAsync();

            var request = new GetTax("Kaunas", "2020-11-08");

            var result = await _handler.Handle(request, default);

            result.ShouldBeNone();
        }
    }
}