using AutoMapper;
using TodoList.Application.DTO.Project;
using TodoList.Domain.Entities;

namespace TodoList.Application.Mappings
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<CreateProjectDTO, Project>()
                .ConvertUsing(dto => new Project(dto.Name, dto.Description));
            CreateMap<Project, ProjectDTO>();
        }
    }
}
