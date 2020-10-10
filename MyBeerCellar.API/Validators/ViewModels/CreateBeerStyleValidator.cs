using FluentValidation;
using MyBeerCellar.API.ViewModels;

namespace MyBeerCellar.API.Validators.ViewModels
{
    public class CreateBeerStyleValidator : AbstractValidator<CreateBeerStyle>
    {
        public CreateBeerStyleValidator()
        {
            RuleFor(_ => _.StyleName)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}