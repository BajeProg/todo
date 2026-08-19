using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoList.API.Validators;
using TodoList.Application.DTO.Project;
using TodoList.Application.UseCases.Project;

namespace TodoList.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private const string GetProjectByIdRoute = "GetProjectById";

        private readonly ProjectService _projectService;
        private readonly IValidator<CreateProjectDTO> _validator;

        public ProjectsController(
            ProjectService projectService,
            IValidator<CreateProjectDTO> validator)
        {
            _projectService = projectService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ProjectDTO>>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            var projects = await _projectService.GetAllAsync(cancellationToken);

            return Ok(projects);
        }

        [HttpGet("{id:guid}", Name = GetProjectByIdRoute)]
        public async Task<ActionResult<ProjectDTO>> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var project = await _projectService.GetByIdAsync(
                id,
                cancellationToken);

            return project is null
                ? NotFound()
                : Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> CreateAsync(
            [FromBody] CreateProjectDTO dto,
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

            var project = await _projectService.CreateAsync(
                dto,
                cancellationToken);

            return CreatedAtRoute(
                GetProjectByIdRoute,
                new { id = project.Id },
                project);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProjectDTO>> UpdateAsync(
            Guid id,
            [FromBody] CreateProjectDTO dto,
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

            var project = await _projectService.UpdateAsync(
                id,
                dto,
                cancellationToken);

            return project is null
                ? NotFound()
                : Ok(project);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var deleted = await _projectService.DeleteAsync(
                id,
                cancellationToken);

            return deleted
                ? NoContent()
                : NotFound();
        }
    }
}
