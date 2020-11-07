using Danske.Producer.API.Features.Taxes;
using Danske.Producer.Application.Taxes.Commands;
using Danske.Producer.Application.Taxes.Queries;
using Danske.Producer.Domain.Tax;
using FluentAssertions;
using LanguageExt;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Danske.Producer.API.Tests.Features.Taxes
{
    [UnitTest]
    public class TaxesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TaxesController _controller;
        private readonly Mock<IImportTaxHandler> _importHandler;

        public TaxesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _importHandler = new Mock<IImportTaxHandler>();
            _controller = new TaxesController(_mediatorMock.Object, _importHandler.Object);
        }

        [Fact]
        public void CreateController_GivenNullMediator_ThrowsArgumentNullException()
        {
            Action action = () => new TaxesController(null, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(RequiredParameters))]
        public async Task GetAsync_GivenValidRequestParameters_ReturnsResponseAsync(GetTaxRequest request)
        {
            GetTax actualQuery = null;

            var expectedQuery = new GetTax(request.Municipality, request.Date);

            var taxMock = new Tax
            {
                Municipality = request.Municipality,
                PeriodType = Enums.PeriodType.Daily,
                PeriodStart = DateTime.Parse(request.Date),
                PeriodEnd = DateTime.Parse(request.Date),
                Result = 0.4m
            };

            var expectedResponse = new GetTaxResponse
            {
                Municipality = request.Municipality,
                PeriodType = Enums.PeriodType.Daily,
                PeriodStart = DateTime.Parse(request.Date),
                PeriodEnd = DateTime.Parse(request.Date),
                Result = 0.5m
            };
            _mediatorMock
                .Setup(s => s.Send(It.IsAny<GetTax>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<Option<Tax>>, CancellationToken>((query, ct) => actualQuery = (GetTax)query)
                .ReturnsAsync(taxMock);

            var response = await _controller.GetAsync(request);

            response.Result.Should().BeOfType<OkObjectResult>();
            _mediatorMock.Verify(s => s.Send(It.IsAny<GetTax>(), It.IsAny<CancellationToken>()), Times.Once);
            actualQuery.Should().Be(expectedQuery);
        }

        public static IEnumerable<object[]> RequiredParameters => new[]
        {
            new[]
            {
                new GetTaxRequest
                {
                    Municipality = "Vilnius",
                    Date = "2020-11-08"
                }
            }
        };
    }
}