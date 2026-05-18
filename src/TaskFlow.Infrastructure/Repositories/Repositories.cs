using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories;

public class TaskRepository : Repository<TaskItem>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext context) : base(context) { }

    public async Task<List<TaskItem>> GetTasksWithDetailsAsync(
        int pageNumber, 
        int pageSize, 
        string? status = null,
        string? priority = null, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking()
            .Include(t => t.Project)
            .Include(t => t.TaskAssignments)
                .ThenInclude(ta => ta.User)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(t => t.Status.ToString() == status);

        if (!string.IsNullOrEmpty(priority))
            query = query.Where(t => t.Priority.ToString() == priority);
        return await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TaskItem>> GetTasksByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Include(t => t.Project)
            .Include(t => t.TaskAssignments)
                .ThenInclude(ta => ta.User)
            .Where(t => t.TaskAssignments.Any(ta => ta.UserId == userId))
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<TaskItem?> GetTaskWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Project)
            .Include(t => t.TaskAssignments)
                .ThenInclude(ta => ta.User)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context) { }

    public async Task<List<Project>> GetActiveProjectsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Project?> GetProjectWithTasksAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Tasks)
                .ThenInclude(t => t.TaskAssignments)
                    .ThenInclude(ta => ta.User)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email.ToLower(), cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Email == email.ToLower(), cancellationToken);
    }
}
