using AutoMapper;
using TodoList.Application.DTO.TaskItem;
using TodoList.Domain.Abstractions;
using TaskItemEntity = TodoList.Domain.Entities.TaskItem;

namespace TodoList.Application.UseCases.TaskItem
{
    public class TaskitemService
    {
        private readonly ITaskItemRepository _taskRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskitemService(
            ITaskItemRepository taskRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _taskRepo = taskRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TaskItemDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var taskItem = await _taskRepo.Get(id, cancellationToken);

            return taskItem is null
                ? null
                : _mapper.Map<TaskItemDto>(taskItem);
        }

        public async Task<IReadOnlyCollection<TaskItemDto>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var taskItems = await _taskRepo.GetAll(cancellationToken);

            return _mapper.Map<List<TaskItemDto>>(taskItems);
        }

        public async Task<IReadOnlyCollection<TaskItemDto>> GetByProjectAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var taskItems = await _taskRepo.GetByProject(projectId, cancellationToken);

            return _mapper.Map<List<TaskItemDto>>(taskItems);
        }

        public async Task<TaskItemDto> CreateAsync(
            CreateTaskItemDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var taskItem = _mapper.Map<TaskItemEntity>(dto);
            var createdTaskItem = await _taskRepo.Add(taskItem, cancellationToken);

            await _unitOfWork.SaveChanges(cancellationToken);

            var persistedTaskItem = await _taskRepo.Get(
                createdTaskItem.Id,
                cancellationToken);

            if (persistedTaskItem is null)
            {
                throw new InvalidOperationException(
                    "The created task item could not be loaded.");
            }

            return _mapper.Map<TaskItemDto>(persistedTaskItem);
        }

        public async Task<TaskItemDto?> UpdateAsync(
            Guid id,
            CreateTaskItemDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var taskItem = await _taskRepo.Get(id, cancellationToken);
            if (taskItem is null)
            {
                return null;
            }

            taskItem.Update(
                dto.Name,
                dto.Description,
                dto.StoryPoints,
                dto.Deadline,
                dto.ProjectId);

            await _taskRepo.Update(taskItem, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            var persistedTaskItem = await _taskRepo.Get(id, cancellationToken);
            if (persistedTaskItem is null)
            {
                throw new InvalidOperationException(
                    "The updated task item could not be loaded.");
            }

            return _mapper.Map<TaskItemDto>(persistedTaskItem);
        }

        public async Task<bool> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var taskItem = await _taskRepo.Get(id, cancellationToken);
            if (taskItem is null)
            {
                return false;
            }

            await _taskRepo.Delete(id, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            return true;
        }
    }
}
