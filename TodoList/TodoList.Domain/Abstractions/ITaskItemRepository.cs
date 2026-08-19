using TodoList.Domain.Entities;

namespace TodoList.Domain.Abstractions
{
    public interface ITaskItemRepository : IRepository<TaskItem>
    {
        public Task<IEnumerable<TaskItem>> GetByProject(Guid projectId, CancellationToken cancellationToken);
    }
}
