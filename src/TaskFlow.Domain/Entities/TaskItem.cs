using TaskFlow.Domain.Common;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.NotStarted;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? Deadline { get; set; }
    public int ProjectId { get; set; }

    // Navigation properties
    public Project Project { get; set; } = null!;
    public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
