using Danske.Producer.API.Features.Taxes;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using Xunit.Categories;

namespace Danske.Producer.API.Tests.Features.Taxes
{
    [UnitTest]
    public class GetTaxRequestValidatorTests
    {
        private readonly GetTaxRequestValidator _validator;

        public GetTaxRequestValidatorTests()
        {
            _validator = new GetTaxRequestValidator();
        }

        [Theory]
        [MemberData(nameof(ValidRequests))]
        public void Validate_WhenRequestIsValid_Passes(GetTaxRequest request)
        {
            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Theory]
        [MemberData(nameof(InvalidMunicipalities))]
        [MemberData(nameof(InvalidDates))]
        public void Validate_WhenRequestIsInvalid_Fails(GetTaxRequest request)
        {
            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        public static IEnumerable<object[]> ValidRequests => new[]
        {
            new[]
            {
                new GetTaxRequest
                {
                    Municipality = "Vilnius",
                    Date = "2020-11-08"
                }
            },
            new[]
            {
                new GetTaxRequest
                {
                    Municipality = "Varena",
                    Date = "2020-11-09"
                }
            },
            new[]
            {
                new GetTaxRequest
                {
                    Municipality = "Kaunas",
                    Date = "2020-11-10"
                }
            },
        };

        public static IEnumerable<object[]> InvalidMunicipalities => new[]
        {
            new[] { new GetTaxRequest { Municipality = "potatopotatopotatopotatopotatopotatopotatopotatopotato" } },
            new[] { new GetTaxRequest { Municipality = "" } }
        };

        public static IEnumerable<object[]> InvalidDates => new[]
        {
            new[] { new GetTaxRequest { Date = "Potato" } },
            new[] { new GetTaxRequest { Date = "" } },
            new[] { new GetTaxRequest { Date = "2020-11-33" } }
        };
    }
}