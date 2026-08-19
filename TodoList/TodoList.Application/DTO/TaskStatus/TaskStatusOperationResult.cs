namespace TodoList.Application.DTO.TaskStatus
{
    public enum TaskStatusOperationStatus
    {
        Success,
        NotFound,
        Protected,
        InUse,
        DuplicateName
    }

    public sealed record TaskStatusOperationResult(
        TaskStatusOperationStatus Status,
        TaskStatusDto? Value = null);
}
