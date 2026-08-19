using FluentValidation;
using TodoList.Application.DTO.TaskItem;

namespace TodoList.API.Validators
{
    public class CreateTaskItemDtoValidator
        : AbstractValidator<CreateTaskItemDto>
    {
        public CreateTaskItemDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.StoryPoints)
                .GreaterThanOrEqualTo(0)
                .When(x => x.StoryPoints.HasValue);

            RuleFor(x => x.ProjectId)
                .NotEmpty();
        }
    }
}
