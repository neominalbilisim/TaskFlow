using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}

public interface ITaskService
{
    Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request, CancellationToken cancellationToken = default);
    Task<TaskResponse> UpdateTaskAsync(int id, UpdateTaskRequest request, CancellationToken cancellationToken = default);
    Task<TaskResponse> UpdateTaskStatusAsync(int id, UpdateTaskStatusRequest request, CancellationToken cancellationToken = default);
    Task DeleteTaskAsync(int id, CancellationToken cancellationToken = default);
    Task<TaskResponse?> GetTaskByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PaginatedResult<TaskResponse>> GetTasksAsync(int pageNumber, int pageSize, string? status = null, string? priority = null, CancellationToken cancellationToken = default);
    Task<List<TaskResponse>> GetMyTasksAsync(int userId, CancellationToken cancellationToken = default);
}

public interface IProjectService
{
    Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request, CancellationToken cancellationToken = default);
    Task<ProjectResponse> UpdateProjectAsync(int id, UpdateProjectRequest request, CancellationToken cancellationToken = default);
    Task<ProjectResponse?> GetProjectByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<ProjectResponse>> GetAllProjectsAsync(CancellationToken cancellationToken = default);
    Task<List<TaskResponse>> GetProjectTasksAsync(int projectId, CancellationToken cancellationToken = default);
}

public interface IUserService
{
    Task<UserResponse?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<UserResponse> UpdateUserRoleAsync(int id, UpdateUserRoleRequest request, CancellationToken cancellationToken = default);
}

public interface ITokenService
{
    string GenerateToken(int userId, string email, UserRole role);
}
