using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoList.API.Validators;
using TodoList.Application.DTO.TaskStatus;
using TodoList.Application.UseCases.TaskStatus;

namespace TodoList.API.Controllers
{
    [ApiController]
    [Route("api/task-statuses")]
    public class TaskStatusesController : ControllerBase
    {
        private const string GetTaskStatusByIdRoute = "GetTaskStatusById";

        private readonly TaskStatusService _service;
        private readonly IValidator<CreateTaskStatusDto> _validator;

        public TaskStatusesController(
            TaskStatusService service,
            IValidator<CreateTaskStatusDto> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<TaskStatusDto>>>
            GetAllAsync(CancellationToken cancellationToken)
        {
            var taskStatuses = await _service.GetAllAsync(cancellationToken);

            return Ok(taskStatuses);
        }

        [HttpGet("{id:guid}", Name = GetTaskStatusByIdRoute)]
        public async Task<ActionResult<TaskStatusDto>> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var taskStatus = await _service.GetByIdAsync(id, cancellationToken);

            return taskStatus is null
                ? NotFound()
                : Ok(taskStatus);
        }

        [HttpPost]
        public async Task<ActionResult<TaskStatusDto>> CreateAsync(
            [FromBody] CreateTaskStatusDto dto,
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

            var result = await _service.CreateAsync(dto, cancellationToken);

            return result.Status switch
            {
                TaskStatusOperationStatus.Success => CreatedAtRoute(
                    GetTaskStatusByIdRoute,
                    new { id = result.Value!.Id },
                    result.Value),
                TaskStatusOperationStatus.DuplicateName => DuplicateNameConflict(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<TaskStatusDto>> UpdateAsync(
            Guid id,
            [FromBody] CreateTaskStatusDto dto,
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

            var result = await _service.UpdateAsync(id, dto, cancellationToken);

            return result.Status switch
            {
                TaskStatusOperationStatus.Success => Ok(result.Value),
                TaskStatusOperationStatus.NotFound => NotFound(),
                TaskStatusOperationStatus.Protected => ProtectedStatusConflict(),
                TaskStatusOperationStatus.DuplicateName => DuplicateNameConflict(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _service.DeleteAsync(id, cancellationToken);

            return result.Status switch
            {
                TaskStatusOperationStatus.Success => NoContent(),
                TaskStatusOperationStatus.NotFound => NotFound(),
                TaskStatusOperationStatus.Protected => ProtectedStatusConflict(),
                TaskStatusOperationStatus.InUse => Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Task status is in use",
                    Detail = "A task status assigned to task items cannot be deleted."
                }),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        private ConflictObjectResult ProtectedStatusConflict()
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Protected task status",
                Detail = "The default 'Открыта' status cannot be changed or deleted."
            });
        }

        private ConflictObjectResult DuplicateNameConflict()
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Task status already exists",
                Detail = "A task status with the same name already exists."
            });
        }
    }
}
