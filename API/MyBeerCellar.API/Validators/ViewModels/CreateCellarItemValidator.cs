using System;
using FluentValidation;
using MyBeerCellar.API.ViewModels;

namespace MyBeerCellar.API.Validators.ViewModels
{
    public class CreateCellarItemValidator : AbstractValidator<CreateCellarItem>
    {
        public CreateCellarItemValidator()
        {
            RuleFor(_ => _.ItemName)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(_ => _.YearProduced)
                .NotEmpty()
                .InclusiveBetween(1950, DateTime.UtcNow.Year);

            RuleFor(_ => _.Quantity)
                .NotEmpty()
                .InclusiveBetween(1, int.MaxValue);

            RuleFor(_ => _.BeerContainerId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(_ => _.BeerStyleId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
