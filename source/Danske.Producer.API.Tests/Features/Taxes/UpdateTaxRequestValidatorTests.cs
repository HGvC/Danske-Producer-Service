using Danske.Producer.API.Features.Taxes;
using Danske.Producer.Enums;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using Xunit.Categories;

namespace Danske.Producer.API.Tests.Features.Taxes
{
    [UnitTest]
    public class UpdateTaxRequestValidatorTests
    {
        private readonly UpdateTaxRequestValidator _validator;

        public UpdateTaxRequestValidatorTests()
        {
            _validator = new UpdateTaxRequestValidator();
        }

        [Theory]
        [MemberData(nameof(ValidRequests))]
        public void Validate_WhenRequestIsValid_Passes(UpdateTaxRequest request)
        {
            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Theory]
        [MemberData(nameof(InvalidMunicipalities))]
        [MemberData(nameof(InvalidDates))]
        [MemberData(nameof(InvalidPeriodType))]
        public void Validate_WhenRequestIsInvalid_Fails(UpdateTaxRequest request)
        {
            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        public static IEnumerable<object[]> ValidRequests => new[]
        {
            new[]
            {
                new UpdateTaxRequest
                {
                    Municipality = "Vilnius",
                    PeriodType = Enums.PeriodType.Yearly,
                    PeriodStart = "2020-01-01",
                    PeriodEnd = "2020-12-31",
                    Tax = 0.1m
                }
            },
            new[]
            {
                new UpdateTaxRequest
                {
                    Municipality = "Vilnius",
                    PeriodType = Enums.PeriodType.Monthly,
                    PeriodStart = "2020-01-01",
                    PeriodEnd = "2020-01-30",
                    Tax = 0.2m
                }
            },
            new[]
            {
                new UpdateTaxRequest
                {
                    Municipality = "Vilnius",
                    PeriodType = Enums.PeriodType.Weekly,
                    PeriodStart = "2020-01-01",
                    PeriodEnd = "2020-01-08",
                    Tax = 0.3m
                }
            },
                        new[]
            {
                new UpdateTaxRequest
                {
                    Municipality = "Vilnius",
                    PeriodType = Enums.PeriodType.Daily,
                    PeriodStart = "2020-01-01",
                    PeriodEnd = "2020-01-01",
                    Tax = 0.4m
                }
            }
        };

        public static IEnumerable<object[]> InvalidMunicipalities => new[]
        {
            new[]
            {
                new UpdateTaxRequest
                {
                    Municipality = "potatopotatopotatopotatopotatopotatopotatopotatopotato",
                    PeriodType = Enums.PeriodType.Daily,
                    PeriodStart = "2020-01-01",
                    PeriodEnd = "2020-01-01",
                    Tax = 0.4m
                }
            },
            new[]
            {
                new UpdateTaxRequest
                {
                    Municipality = "",
                    PeriodType = Enums.PeriodType.Daily,
                    PeriodStart = "2020-01-01",
                    PeriodEnd = "2020-01-01",
                    Tax = 0.4m
                }
            }
        };

        public static IEnumerable<object[]> InvalidDates => new[]
        {
            new[]
            {
                new UpdateTaxRequest
                {
                    Municipality = "Vilnius",
                    PeriodType = Enums.PeriodType.Daily,
                    PeriodStart = "",
                    PeriodEnd = "2020-01-01",
                    Tax = 0.4m
                }
            },
            new[]
            {
                new UpdateTaxRequest
                {
                    Municipality = "Vilnius",
                    PeriodType = Enums.PeriodType.Daily,
                    PeriodStart = "2020-01-01",
                    PeriodEnd = "Potat",
                    Tax = 0.4m
                }
            },
            new[]
            {
                new UpdateTaxRequest
                {
                    Municipality = "Vilnius",
                    PeriodType = Enums.PeriodType.Daily,
                    PeriodStart = "2020-01-03",
                    PeriodEnd = "2020-01-02",
                    Tax = 0.4m
                }
            }
        };

        public static IEnumerable<object[]> InvalidPeriodType => new[]
{
            new[]
            {
                new UpdateTaxRequest
                {
                    Municipality = "Vilnius",
                    PeriodType = (PeriodType)5,
                    PeriodStart = "",
                    PeriodEnd = "2020-01-01",
                    Tax = 0.4m
                }
            },
            new[]
            {
                new UpdateTaxRequest
                {
                    Municipality = "Vilnius",
                    PeriodStart = "",
                    PeriodEnd = "2020-01-01",
                    Tax = 0.4m
                }
            }
        };
    }
}