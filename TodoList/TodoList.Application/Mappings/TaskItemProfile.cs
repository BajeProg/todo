using AutoMapper;
using TodoList.Application.DTO.TaskItem;
using TodoList.Domain.Entities;

namespace TodoList.Application.Mappings
{
    public class TaskItemProfile : Profile
    {
        public TaskItemProfile()
        {
            CreateMap<CreateTaskItemDto, TaskItem>()
                .ConvertUsing(dto => new TaskItem(
                    dto.Name,
                    dto.Description,
                    dto.StoryPoints,
                    dto.Deadline,
                    dto.ProjectId));
            CreateMap<TaskItem, TaskItemDto>();
        }
    }
}
