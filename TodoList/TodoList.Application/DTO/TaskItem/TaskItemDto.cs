using TodoList.Application.DTO.Project;

namespace TodoList.Application.DTO.TaskItem
{
    public class TaskItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? StoryPoints { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Deadline { get; set; }
        public ProjectDTO Project { get; set; } = null!;
    }
}
