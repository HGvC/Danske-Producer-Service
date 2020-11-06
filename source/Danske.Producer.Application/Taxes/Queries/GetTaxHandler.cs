using Danske.Producer.Domain.Tax;
using Danske.Producer.Infrastructure;
using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Danske.Producer.Application.Taxes.Queries
{
    public class GetTaxHandler : IRequestHandler<GetTax, Option<Tax>>
    {
        private readonly TaxesDbContext _dbContext;

        public GetTaxHandler(TaxesDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Option<Tax>> Handle(GetTax request, CancellationToken cancellationToken)
        {
            return await _dbContext.Taxes
                .AsNoTracking()
                .Where(s => s.Municipality == request.Municipality && s.PeriodStart <= request.Date && s.PeriodEnd >= request.Date)
                .OrderByDescending(s => s.PeriodType)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}