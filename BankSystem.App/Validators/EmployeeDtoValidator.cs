using BankSystem.App.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Validators
{
    public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeDtoValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(e => e.FullName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Имя пользователя обязательно для ввода");

            RuleFor(e => e.PassportData)
               .NotNull()
               .NotEmpty()
               .WithMessage("Паспортные данные обязательны для ввода")
               .Length(11);

            RuleFor(e => e.BirthDate)
               .NotNull()
               .NotEmpty()
               .WithMessage("Дата рождения обязательна для ввода");

            RuleFor(e => e.Salary)
                .NotNull()
                .NotEmpty()
                .WithMessage("Заработная плата обязательна для ввода");

            RuleFor(e => e.Position)
                .NotNull()
                .NotEmpty()
                .WithMessage("Должность обязательна для ввода");

            RuleFor(e => e.Contract)
                .NotNull()
                .NotEmpty()
                .WithMessage("Контракт обязателен для ввода");
        }
    }
}
