namespace TodoList.Domain.Entities
{
    public class TaskItem
    {
        private TaskItem()
        {
        }

        public TaskItem(
            string name,
            string? description,
            int? storyPoints,
            DateTime? deadline,
            Guid projectId,
            Guid statusId)
        {
            CreatedAt = DateTime.UtcNow;

            var initialStatusId = statusId == Guid.Empty
                ? TaskStatus.OpenId
                : statusId;

            Update(
                name,
                description,
                storyPoints,
                deadline,
                projectId,
                initialStatusId);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }
        public int? StoryPoints { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? Deadline { get; private set; }
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; } = null!;
        public Guid StatusId { get; private set; }
        public TaskStatus Status { get; private set; } = null!;

        public void Update(
            string name,
            string? description,
            int? storyPoints,
            DateTime? deadline,
            Guid projectId,
            Guid statusId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            if (storyPoints is < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(storyPoints),
                    "Story points cannot be negative.");
            }

            if (projectId == Guid.Empty)
            {
                throw new ArgumentException(
                    "Project identifier cannot be empty.",
                    nameof(projectId));
            }

            if (statusId == Guid.Empty)
            {
                throw new ArgumentException(
                    "Status identifier cannot be empty.",
                    nameof(statusId));
            }

            if (ProjectId != Guid.Empty && ProjectId != projectId)
            {
                Project = null!;
            }

            if (StatusId != Guid.Empty && StatusId != statusId)
            {
                Status = null!;
            }

            Name = name.Trim();
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim();
            StoryPoints = storyPoints;
            Deadline = deadline;
            ProjectId = projectId;
            StatusId = statusId;
        }
    }
}
