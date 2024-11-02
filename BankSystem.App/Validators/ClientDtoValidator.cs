using BankSystem.App.Dto;
using FluentValidation;
using System.Data;

namespace BankSystem.App.Validators
{
    public class ClientDtoValidator : AbstractValidator<ClientDto>
    {
        public ClientDtoValidator()
        {
            RuleFor(c => c.FullName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Имя пользователя обязательно для ввода");

            RuleFor(c => c.PassportData)
               .NotNull()
               .NotEmpty()
               .WithMessage("Паспортные данные обязательны для ввода")
               .Length(11);

            RuleFor(c => c.BirthDate)
               .NotNull()
               .NotEmpty()
               .WithMessage("Дата рождения обязательна для ввода");

            RuleFor(c => c.TelephoneNumber)
               .NotNull()
               .NotEmpty()
               .WithMessage("Номер телефона обязателен для ввода");
        }
    }
}
