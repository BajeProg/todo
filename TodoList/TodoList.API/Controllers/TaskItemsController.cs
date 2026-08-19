using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoList.API.Validators;
using TodoList.Application.DTO.TaskItem;
using TodoList.Application.UseCases.TaskItem;

namespace TodoList.API.Controllers
{
    [ApiController]
    [Route("api/task-items")]
    public class TaskItemsController : ControllerBase
    {
        private const string GetTaskItemByIdRoute = "GetTaskItemById";

        private readonly TaskitemService _taskItemService;
        private readonly IValidator<CreateTaskItemDto> _validator;

        public TaskItemsController(
            TaskitemService taskItemService,
            IValidator<CreateTaskItemDto> validator)
        {
            _taskItemService = taskItemService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<TaskItemDto>>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            var taskItems = await _taskItemService.GetAllAsync(cancellationToken);

            return Ok(taskItems);
        }

        [HttpGet("{id:guid}", Name = GetTaskItemByIdRoute)]
        public async Task<ActionResult<TaskItemDto>> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var taskItem = await _taskItemService.GetByIdAsync(
                id,
                cancellationToken);

            return taskItem is null
                ? NotFound()
                : Ok(taskItem);
        }

        [HttpGet("~/api/projects/{projectId:guid}/task-items")]
        public async Task<ActionResult<IReadOnlyCollection<TaskItemDto>>>
            GetByProjectAsync(
                Guid projectId,
                CancellationToken cancellationToken)
        {
            var taskItems = await _taskItemService.GetByProjectAsync(
                projectId,
                cancellationToken);

            return Ok(taskItems);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItemDto>> CreateAsync(
            [FromBody] CreateTaskItemDto dto,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(
                dto,
                cancellationToken);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return ValidationProblem(ModelState);
            }

            var taskItem = await _taskItemService.CreateAsync(
                dto,
                cancellationToken);

            return CreatedAtRoute(
                GetTaskItemByIdRoute,
                new { id = taskItem.Id },
                taskItem);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<TaskItemDto>> UpdateAsync(
            Guid id,
            [FromBody] CreateTaskItemDto dto,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(
                dto,
                cancellationToken);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return ValidationProblem(ModelState);
            }

            var taskItem = await _taskItemService.UpdateAsync(
                id,
                dto,
                cancellationToken);

            return taskItem is null
                ? NotFound()
                : Ok(taskItem);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var deleted = await _taskItemService.DeleteAsync(
                id,
                cancellationToken);

            return deleted
                ? NoContent()
                : NotFound();
        }
    }
}
