using BankSystem.App.Dto;
using FluentValidation;
using System;
using System.Data;

namespace BankSystem.App.Validators
{
    public class ClientDtoValidator : AbstractValidator<ClientDto>
    {
        public ClientDtoValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .NotEmpty();

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
                 .Must(birthDate => CalculateAge(birthDate) >= 18)
                 .WithMessage("Клиент должен быть не моложе 18 лет");

            RuleFor(c => c.TelephoneNumber)
               .NotNull()
               .NotEmpty()
               .WithMessage("Номер телефона обязателен для ввода");


        }
        private int CalculateAge(DateTimeOffset? birthDate)
        {
            if (!birthDate.HasValue) return 0;
            var age = DateTime.Today.Year - birthDate.Value.Year;
            if (birthDate.Value.Date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }
    }
}
