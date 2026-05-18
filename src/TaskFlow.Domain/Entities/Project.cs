using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    // Navigation properties
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
