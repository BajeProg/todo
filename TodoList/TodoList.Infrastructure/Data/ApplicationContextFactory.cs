using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TodoList.Infrastructure.Data
{
    public class ApplicationContextFactory
        : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable(
                "ConnectionStrings__Database")
                ?? Environment.GetEnvironmentVariable(
                    "TODO_LIST_CONNECTION_STRING")
                ?? "Host=localhost;Database=todo_list;Username=postgres";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseNpgsql(
                connectionString,
                options => options.MigrationsAssembly(
                    typeof(ApplicationContext).Assembly.FullName));

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
