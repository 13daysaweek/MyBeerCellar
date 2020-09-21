using FluentValidation;
using MyBeerCellar.API.ViewModels;

namespace MyBeerCellar.API.Validators.ViewModels
{
    public class UpdateBeerStyleValidator : AbstractValidator<UpdateBeerStyle>
    {
        public UpdateBeerStyleValidator()
        {
            RuleFor(_ => _.StyleId)
                .NotEmpty();

            RuleFor(_ => _.StyleName)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}