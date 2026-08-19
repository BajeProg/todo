using AutoMapper;
using TodoList.Application.DTO.TaskStatus;
using TaskStatusEntity = TodoList.Domain.Entities.TaskStatus;

namespace TodoList.Application.Mappings
{
    public class TaskStatusProfile : Profile
    {
        public TaskStatusProfile()
        {
            CreateMap<CreateTaskStatusDto, TaskStatusEntity>()
                .ConvertUsing(dto => new TaskStatusEntity(dto.Name, dto.Color));
            CreateMap<TaskStatusEntity, TaskStatusDto>();
        }
    }
}
