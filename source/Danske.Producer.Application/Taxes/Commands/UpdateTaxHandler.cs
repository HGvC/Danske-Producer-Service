using Danske.Producer.Application.Common;
using Danske.Producer.Domain.Tax;
using Danske.Producer.Infrastructure;
using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unit = LanguageExt.Unit;

namespace Danske.Producer.Application.Taxes.Commands
{
    public class UpdateTaxHandler : IRequestHandler<UpdateTax, Either<Failure, Unit>>
    {
        private readonly TaxesDbContext _dbContext;

        public UpdateTaxHandler(TaxesDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Either<Failure, Unit>> Handle(UpdateTax request, CancellationToken cancellationToken)
        {
            var taxToUpdate = await _dbContext.Taxes
                .Where(s => s.Municipality == request.Municipality &&
                    s.PeriodType == request.PeriodType &&
                    s.PeriodStart == request.PeriodStart &&
                    s.PeriodEnd == request.PeriodEnd)
                .FirstOrDefaultAsync(cancellationToken);

            if (taxToUpdate == null)
            {
                taxToUpdate = setTaxToUpdate(request);
                _dbContext.Taxes.Add(taxToUpdate);
            }

            taxToUpdate.Result = request.Tax;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Default;
        }

        private static Tax setTaxToUpdate(UpdateTax request)
        {
            return new Tax()
            {
                Municipality = request.Municipality,
                PeriodType = request.PeriodType,
                PeriodStart = request.PeriodStart,
                PeriodEnd = request.PeriodEnd,
                Result = request.Tax
            };
        }
    }
}