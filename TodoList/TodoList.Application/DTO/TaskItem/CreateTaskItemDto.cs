namespace TodoList.Application.DTO.TaskItem
{
    public class CreateTaskItemDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? StoryPoints { get; set; }
        public DateTime? Deadline { get; set; }
        public Guid StatusId { get; set; }
        public Guid ProjectId { get; set; }
    }
}
