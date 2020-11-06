using Danske.Producer.Application.Common;
using Danske.Producer.Enums;
using LanguageExt;
using MediatR;
using System;
using Unit = LanguageExt.Unit;

namespace Danske.Producer.Application.Taxes.Commands
{
    public class UpdateTax : Record<UpdateTax>, IRequest<Either<Failure, Unit>>
    {
        public string Municipality { get; }
        public PeriodType PeriodType { get; }
        public DateTime PeriodStart { get; }
        public DateTime PeriodEnd { get; }
        public decimal Tax { get; }

        public UpdateTax(string municipality, PeriodType periodType, string periodStart, string periodEnd, decimal tax)
        {
            Municipality = municipality;
            PeriodType = periodType;
            PeriodStart = DateTime.Parse(periodStart);
            PeriodEnd = DateTime.Parse(periodEnd);
            Tax = tax;
        }
    }
}