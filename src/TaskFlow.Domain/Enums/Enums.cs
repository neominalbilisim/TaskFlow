namespace TaskFlow.Domain.Enums;

public enum TaskStatus
{
    NotStarted = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3
}

public enum TaskPriority
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}

public enum UserRole
{
    Employee = 0,
    Manager = 1,
    Admin = 2
}
