namespace TodoList.Application.DTO.Project
{
    public class ProjectDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
