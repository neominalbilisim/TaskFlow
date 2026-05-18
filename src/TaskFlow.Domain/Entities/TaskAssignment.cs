using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class TaskAssignment : BaseEntity
{
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public TaskItem Task { get; set; } = null!;
    public User User { get; set; } = null!;
}
