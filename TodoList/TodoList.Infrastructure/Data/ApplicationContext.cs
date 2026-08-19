using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities;
using TaskStatusEntity = TodoList.Domain.Entities.TaskStatus;

namespace TodoList.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<TaskStatusEntity> TaskStatuses => Set<TaskStatusEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationContext).Assembly);
        }
    }
}
