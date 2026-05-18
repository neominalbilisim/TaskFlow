using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs;

// Auth DTOs
public record RegisterRequest(string Email, string Password, string FirstName, string LastName);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, string Email, string Role);

// Task DTOs
public record CreateTaskRequest(string Title, string Description, int ProjectId, TaskPriority Priority, DateTime? Deadline, List<int> AssignedUserIds);
public record UpdateTaskRequest(string Title, string Description, TaskPriority Priority, DateTime? Deadline);
public record UpdateTaskStatusRequest(Domain.Enums.TaskStatus Status);
public record TaskResponse(int Id, string Title, string Description, Domain.Enums.TaskStatus Status, TaskPriority Priority, DateTime? Deadline, int ProjectId, string ProjectName, List<AssignedUserDto> AssignedUsers, DateTime CreatedAt);
public record AssignedUserDto(int UserId, string Email, string FullName);

// Project DTOs
public record CreateProjectRequest(string Name, string Description, DateTime? StartDate, DateTime? EndDate);
public record UpdateProjectRequest(string Name, string Description, bool IsActive, DateTime? StartDate, DateTime? EndDate);
public record ProjectResponse(int Id, string Name, string Description, bool IsActive, DateTime? StartDate, DateTime? EndDate, int TaskCount);

// User DTOs
public record UserResponse(int Id, string Email, string FirstName, string LastName, UserRole Role, bool IsActive);
public record UpdateUserRoleRequest(UserRole Role);

// Common DTOs
public record PaginatedResult<T>(List<T> Items, int TotalCount, int PageNumber, int PageSize, int TotalPages);
