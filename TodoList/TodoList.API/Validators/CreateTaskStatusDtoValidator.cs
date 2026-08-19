using FluentValidation;
using TodoList.Application.DTO.TaskStatus;

namespace TodoList.API.Validators
{
    public class CreateTaskStatusDtoValidator
        : AbstractValidator<CreateTaskStatusDto>
    {
        public CreateTaskStatusDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Color)
                .NotEmpty()
                .Matches("^#[0-9A-Fa-f]{6}$")
                .WithMessage("Color must be in #RRGGBB format.");
        }
    }
}
