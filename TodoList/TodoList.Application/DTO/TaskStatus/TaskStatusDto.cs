namespace TodoList.Application.DTO.TaskStatus
{
    public class TaskStatusDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Color { get; set; } = null!;
        public bool IsSystem { get; set; }
    }
}
