namespace TodoList.Domain.Entities
{
    public class TaskStatus
    {
        public static readonly Guid OpenId = Guid.Parse(
            "00000000-0000-0000-0000-000000000001");

        public const string OpenName = "Открыта";
        public const string OpenColor = "#3B82F6";

        private TaskStatus()
        {
        }

        public TaskStatus(string name, string color)
        {
            Update(name, color);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = null!;
        public string NormalizedName { get; private set; } = null!;
        public string Color { get; private set; } = null!;
        public bool IsSystem { get; private set; }

        public void Update(string name, string color)
        {
            if (IsSystem || Id == OpenId)
            {
                throw new InvalidOperationException(
                    "A system task status cannot be modified.");
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(color);

            Name = name.Trim();
            NormalizedName = NormalizeName(name);
            Color = color.Trim().ToUpperInvariant();
        }

        public static string NormalizeName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            return name.Trim().ToUpperInvariant();
        }
    }
}
