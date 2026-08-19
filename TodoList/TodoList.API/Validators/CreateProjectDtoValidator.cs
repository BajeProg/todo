using FluentValidation;
using TodoList.Application.DTO.Project;

namespace TodoList.API.Validators
{
    public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDTO>
    {
        public CreateProjectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(2000);
        }
    }
}
