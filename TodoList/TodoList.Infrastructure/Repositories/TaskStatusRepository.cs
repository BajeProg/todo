using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Abstractions;
using TodoList.Infrastructure.Data;
using TaskStatusEntity = TodoList.Domain.Entities.TaskStatus;

namespace TodoList.Infrastructure.Repositories
{
    public class TaskStatusRepository(ApplicationContext context)
        : ITaskStatusRepository
    {
        public async Task<TaskStatusEntity> Add(
            TaskStatusEntity entity,
            CancellationToken cancellationToken)
        {
            await context.TaskStatuses.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var entity = await context.TaskStatuses.FindAsync(
                [id],
                cancellationToken);

            if (entity is not null)
            {
                context.TaskStatuses.Remove(entity);
            }
        }

        public async Task<TaskStatusEntity?> Get(
            Guid id,
            CancellationToken cancellationToken)
        {
            return await context.TaskStatuses
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<TaskStatusEntity>> GetAll(
            CancellationToken cancellationToken)
        {
            return await context.TaskStatuses
                .AsNoTracking()
                .OrderByDescending(x => x.IsSystem)
                .ThenBy(x => x.Name)
                .ToListAsync(cancellationToken);
        }

        public Task<TaskStatusEntity> Update(
            TaskStatusEntity entity,
            CancellationToken cancellationToken)
        {
            var entry = context.TaskStatuses.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                context.TaskStatuses.Attach(entity);
            }

            entry.State = EntityState.Modified;
            entry.Property(x => x.IsSystem).IsModified = false;

            return Task.FromResult(entity);
        }

        public Task<bool> IsInUse(
            Guid id,
            CancellationToken cancellationToken)
        {
            return context.Tasks.AnyAsync(
                x => x.StatusId == id,
                cancellationToken);
        }

        public Task<bool> NameExists(
            string name,
            Guid? excludedId,
            CancellationToken cancellationToken)
        {
            var normalizedName = TaskStatusEntity.NormalizeName(name);

            return context.TaskStatuses.AnyAsync(
                x => x.NormalizedName == normalizedName
                    && (!excludedId.HasValue || x.Id != excludedId.Value),
                cancellationToken);
        }
    }
}
