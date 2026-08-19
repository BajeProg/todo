namespace TodoList.Domain.Entities
{
    public class Project
    {
        private Project()
        {
        }

        public Project(string name, string? description)
        {
            Update(name, description);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }

        public void Update(string name, string? description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name.Trim();
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim();
        }
    }
}
