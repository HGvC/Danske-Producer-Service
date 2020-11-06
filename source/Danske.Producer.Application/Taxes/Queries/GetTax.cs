using Danske.Producer.Domain.Tax;
using LanguageExt;
using MediatR;
using System;

namespace Danske.Producer.Application.Taxes.Queries
{
    public class GetTax : Record<GetTax>, IRequest<Option<Tax>>
    {
        public string Municipality { get; }

        public DateTime Date { get; }

        public GetTax(string municipality, string date)
        {
            Municipality = municipality;
            Date = DateTime.Parse(date);
        }
    }
}