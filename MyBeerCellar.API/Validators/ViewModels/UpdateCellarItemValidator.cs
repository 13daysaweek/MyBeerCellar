using System;
using FluentValidation;
using MyBeerCellar.API.ViewModels;

namespace MyBeerCellar.API.Validators.ViewModels
{
    public class UpdateCellarItemValidator : AbstractValidator<UpdateCellarItem>
    {
        public UpdateCellarItemValidator()
        {
            RuleFor(_ => _.CellarItemId)
                .NotEmpty();

            RuleFor(_ => _.ItemName)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(_ => _.YearProduced)
                .NotEmpty()
                .InclusiveBetween(1950, DateTime.UtcNow.Year);

            RuleFor(_ => _.Quantity)
                .NotEmpty();

            RuleFor(_ => _.BeerStyleId)
                .NotEmpty();

            RuleFor(_ => _.BeerContainerId)
                .NotEmpty();
        }
    }
}
