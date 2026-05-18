using TaskFlow.Domain.Common;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Employee;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
