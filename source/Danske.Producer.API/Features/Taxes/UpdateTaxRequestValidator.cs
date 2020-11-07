using FluentValidation;
using System;

namespace Danske.Producer.API.Features.Taxes
{
    public class UpdateTaxRequestValidator : AbstractValidator<UpdateTaxRequest>
    {
        public UpdateTaxRequestValidator()
        {
            RuleFor(s => s.Municipality).MaximumLength(50);
            RuleFor(s => s.Municipality).NotEmpty();
            RuleFor(s => s.PeriodType).IsInEnum().WithMessage("PeriodType has to be in range from 1 to 4");
            RuleFor(s => s.PeriodType).NotEmpty();
            RuleFor(s => s.PeriodStart).Must(BeAValidDate).WithMessage("Invalid date format, example: 2020-11-08");
            RuleFor(s => s.PeriodStart).LessThanOrEqualTo(s => s.PeriodEnd);
            RuleFor(s => s.PeriodEnd).Must(BeAValidDate).WithMessage("Invalid date format, example: 2020-11-08");
        }

        private bool BeAValidDate(string value)
        {
            return DateTime.TryParse(value, out var date);
        }
    }
}