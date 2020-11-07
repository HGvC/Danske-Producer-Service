using FluentValidation;
using System;

namespace Danske.Producer.API.Features.Taxes
{
    public class GetTaxRequestValidator : AbstractValidator<GetTaxRequest>
    {
        public GetTaxRequestValidator()
        {
            RuleFor(s => s.Municipality).NotEmpty();
            RuleFor(s => s.Municipality).MaximumLength(50);
            RuleFor(s => s.Date).NotEmpty();
            RuleFor(s => s.Date).Must(BeAValidDate).WithMessage("Invalid date format, example: 2020-11-08");
        }

        private bool BeAValidDate(string value)
        {
            return DateTime.TryParse(value, out var date);
        }
    }
}