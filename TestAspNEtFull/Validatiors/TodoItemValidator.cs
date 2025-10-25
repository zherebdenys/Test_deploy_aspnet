using System;
using FluentValidation;
using TestAspNEtFull.Models;

namespace TestAspNEtFull.Validatiors;

public class TodoItemValidator : AbstractValidator<TodoItem>
{
    public TodoItemValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty().WithMessage("Поле 'Name' є обов'язковим.")
            .MinimumLength(3).WithMessage("Назва має містити щонайменше 3 символи.")
            .MaximumLength(100).WithMessage("Назва не може перевищувати 100 символів.");

        RuleFor(x => x.IsComplete)
            .NotNull().WithMessage("Поле 'IsComplete' має бути вказано.");
    }
}
