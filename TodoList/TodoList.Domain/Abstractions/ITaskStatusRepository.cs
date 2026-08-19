using TodoList.Domain.Entities;
using TaskStatusEntity = TodoList.Domain.Entities.TaskStatus;

namespace TodoList.Domain.Abstractions
{
    public interface ITaskStatusRepository : IRepository<TaskStatusEntity>
    {
        Task<bool> IsInUse(Guid id, CancellationToken cancellationToken);

        Task<bool> NameExists(
            string name,
            Guid? excludedId,
            CancellationToken cancellationToken);
    }
}
