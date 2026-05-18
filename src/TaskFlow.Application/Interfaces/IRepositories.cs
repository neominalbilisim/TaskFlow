using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Interfaces;

public interface ITaskRepository : IRepository<TaskItem>
{
    Task<List<TaskItem>> GetTasksWithDetailsAsync(int pageNumber, int pageSize, string? status = null, string? priority = null, CancellationToken cancellationToken = default);
    Task<List<TaskItem>> GetTasksByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<TaskItem?> GetTaskWithDetailsAsync(int id, CancellationToken cancellationToken = default);
}

public interface IProjectRepository : IRepository<Project>
{
    Task<List<Project>> GetActiveProjectsAsync(CancellationToken cancellationToken = default);
    Task<Project?> GetProjectWithTasksAsync(int id, CancellationToken cancellationToken = default);
}

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}
