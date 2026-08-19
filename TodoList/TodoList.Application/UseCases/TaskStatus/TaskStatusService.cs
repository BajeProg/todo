using AutoMapper;
using TodoList.Application.DTO.TaskStatus;
using TodoList.Domain.Abstractions;
using TaskStatusEntity = TodoList.Domain.Entities.TaskStatus;

namespace TodoList.Application.UseCases.TaskStatus
{
    public class TaskStatusService
    {
        private readonly ITaskStatusRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskStatusService(
            ITaskStatusRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TaskStatusDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var taskStatus = await _repository.Get(id, cancellationToken);

            return taskStatus is null
                ? null
                : _mapper.Map<TaskStatusDto>(taskStatus);
        }

        public async Task<IReadOnlyCollection<TaskStatusDto>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var taskStatuses = await _repository.GetAll(cancellationToken);

            return _mapper.Map<List<TaskStatusDto>>(taskStatuses);
        }

        public async Task<TaskStatusOperationResult> CreateAsync(
            CreateTaskStatusDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (await _repository.NameExists(
                dto.Name.Trim(),
                excludedId: null,
                cancellationToken))
            {
                return new(TaskStatusOperationStatus.DuplicateName);
            }

            var taskStatus = _mapper.Map<TaskStatusEntity>(dto);
            var createdTaskStatus = await _repository.Add(
                taskStatus,
                cancellationToken);

            await _unitOfWork.SaveChanges(cancellationToken);

            return new(
                TaskStatusOperationStatus.Success,
                _mapper.Map<TaskStatusDto>(createdTaskStatus));
        }

        public async Task<TaskStatusOperationResult> UpdateAsync(
            Guid id,
            CreateTaskStatusDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var taskStatus = await _repository.Get(id, cancellationToken);
            if (taskStatus is null)
            {
                return new(TaskStatusOperationStatus.NotFound);
            }

            if (taskStatus.IsSystem || taskStatus.Id == TaskStatusEntity.OpenId)
            {
                return new(TaskStatusOperationStatus.Protected);
            }

            if (await _repository.NameExists(
                dto.Name.Trim(),
                excludedId: id,
                cancellationToken))
            {
                return new(TaskStatusOperationStatus.DuplicateName);
            }

            taskStatus.Update(dto.Name, dto.Color);

            var updatedTaskStatus = await _repository.Update(
                taskStatus,
                cancellationToken);

            await _unitOfWork.SaveChanges(cancellationToken);

            return new(
                TaskStatusOperationStatus.Success,
                _mapper.Map<TaskStatusDto>(updatedTaskStatus));
        }

        public async Task<TaskStatusOperationResult> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var taskStatus = await _repository.Get(id, cancellationToken);
            if (taskStatus is null)
            {
                return new(TaskStatusOperationStatus.NotFound);
            }

            if (taskStatus.IsSystem || taskStatus.Id == TaskStatusEntity.OpenId)
            {
                return new(TaskStatusOperationStatus.Protected);
            }

            if (await _repository.IsInUse(id, cancellationToken))
            {
                return new(TaskStatusOperationStatus.InUse);
            }

            await _repository.Delete(id, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            return new(TaskStatusOperationStatus.Success);
        }
    }
}
