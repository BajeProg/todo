using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Abstractions;
using TodoList.Domain.Entities;
using TodoList.Infrastructure.Data;

namespace TodoList.Infrastructure.Repositories
{
    public class TaskItemRepository(ApplicationContext _context) : ITaskItemRepository
    {
        public async Task<TaskItem> Add(
            TaskItem entity,
            CancellationToken cancellationToken)
        {
            await _context.Tasks.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.Tasks.FindAsync([id], cancellationToken);
            if (entity is not null)
            {
                _context.Tasks.Remove(entity);
            }
        }

        public async Task<TaskItem?> Get(
            Guid id,
            CancellationToken cancellationToken)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Include(x => x.Project)
                .Include(x => x.Status)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>> GetAll(
            CancellationToken cancellationToken)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Include(x => x.Project)
                .Include(x => x.Status)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>> GetByProject(
            Guid projectId,
            CancellationToken cancellationToken)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Include(x => x.Project)
                .Include(x => x.Status)
                .Where(x => x.ProjectId == projectId)
                .ToListAsync(cancellationToken);
        }

        public Task<TaskItem> Update(
            TaskItem entity,
            CancellationToken cancellationToken)
        {
            var entry = _context.Tasks.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                _context.Tasks.Attach(entity);
            }

            entry.State = EntityState.Modified;

            return Task.FromResult(entity);
        }
    }
}
