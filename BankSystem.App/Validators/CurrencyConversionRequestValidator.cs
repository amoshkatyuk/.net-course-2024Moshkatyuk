using FluentValidation;
using BankSystem.Domain.Models;

namespace BankSystem.App.Validators
{
    public class CurrencyConversionRequestValidator : AbstractValidator<CurrencyConversionRequest>
    {
        public CurrencyConversionRequestValidator()
        {
            RuleFor(r => r.FromCurrency)
                .NotNull()
                .NotEmpty().WithMessage("Отсутствует валюта отправления");

            RuleFor(r => r.ToCurrency)
                .NotNull()
                .NotEmpty().WithMessage("Отсутствует валбта получения");

            RuleFor(r => r.Amount)
                .GreaterThan(0).WithMessage("Сумма должна быть больше нуля");
        }
    }
}
