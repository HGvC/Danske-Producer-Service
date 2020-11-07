using Danske.Producer.Application.Taxes.Commands;
using Danske.Producer.Domain.Tax;

namespace Danske.Producer.API.Features.Taxes
{
    internal static class GetTaxMapper
    {
        internal static GetTaxResponse ToResponse(this Tax entity)
        {
            return new GetTaxResponse
            {
                Municipality = entity.Municipality,
                PeriodType = entity.PeriodType,
                PeriodStart = entity.PeriodStart,
                PeriodEnd = entity.PeriodEnd,
                Result = entity.Result
            };
        }

        internal static UpdateTax ToCommand(this UpdateTaxRequest request)
        {
            return new UpdateTax(
               request.Municipality,
               request.PeriodType,
               request.PeriodStart,
               request.PeriodEnd,
               request.Tax);
        }
    }
}