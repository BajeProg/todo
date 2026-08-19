using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Abstractions;
using TodoList.Domain.Entities;
using TodoList.Infrastructure.Data;

namespace TodoList.Infrastructure.Repositories
{
    public class ProjectRepository(ApplicationContext _context) : IRepository<Project>
    {
        public async Task<Project> Add(Project entity, CancellationToken cancellationToken)
        {
            await _context.Projects.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.Projects.FindAsync([id], cancellationToken);
            if (entity is not null)
            {
                _context.Projects.Remove(entity);
            }
        }

        public async Task<Project?> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Projects.FindAsync([id], cancellationToken);
        }

        public async Task<IEnumerable<Project>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Projects.ToListAsync(cancellationToken);
        }

        public Task<Project> Update(Project entity, CancellationToken cancellationToken)
        {
            var entry = _context.Projects.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                _context.Projects.Update(entity);
            }

            return Task.FromResult(entity);
        }
    }
}
