using Microsoft.Extensions.DependencyInjection;
using TodoList.Application.UseCases.Project;
using TodoList.Application.UseCases.TaskItem;

namespace TodoList.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddAutoMapper(
                configuration => { },
                typeof(DependencyInjection).Assembly);

            services.AddScoped<ProjectService>();
            services.AddScoped<TaskitemService>();

            return services;
        }
    }
}
