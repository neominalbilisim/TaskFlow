using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IRepository<TaskAssignment> _taskAssignmentRepository;

    public TaskService(
        ITaskRepository taskRepository,
        IUserRepository userRepository,
        IProjectRepository projectRepository,
        IRepository<TaskAssignment> taskAssignmentRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _taskAssignmentRepository = taskAssignmentRepository;
    }

    public async Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null)
            throw new NotFoundException("Project not found");

        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            Deadline = request.Deadline,
            ProjectId = request.ProjectId,
            Status = Domain.Enums.TaskStatus.NotStarted
        };

        await _taskRepository.AddAsync(task, cancellationToken);

        foreach (var userId in request.AssignedUserIds)
        {
            var assignment = new TaskAssignment
            {
                TaskId = task.Id,
                UserId = userId
            };
            await _taskAssignmentRepository.AddAsync(assignment, cancellationToken);
        }

        return await GetTaskByIdAsync(task.Id, cancellationToken) 
            ?? throw new NotFoundException("Task not found");
    }

    public async Task<TaskResponse> UpdateTaskAsync(int id, UpdateTaskRequest request, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
        if (task == null)
            throw new NotFoundException("Task not found");

        task.Title = request.Title;
        task.Description = request.Description;
        task.Priority = request.Priority;
        task.Deadline = request.Deadline;

        await _taskRepository.UpdateAsync(task, cancellationToken);

        return await GetTaskByIdAsync(id, cancellationToken) 
            ?? throw new NotFoundException("Task not found");
    }

    public async Task<TaskResponse> UpdateTaskStatusAsync(int id, UpdateTaskStatusRequest request, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
        if (task == null)
            throw new NotFoundException("Task not found");

        task.Status = request.Status;
        await _taskRepository.UpdateAsync(task, cancellationToken);

        return await GetTaskByIdAsync(id, cancellationToken) 
            ?? throw new NotFoundException("Task not found");
    }

    public async Task DeleteTaskAsync(int id, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
        if (task == null)
            throw new NotFoundException("Task not found");

        await _taskRepository.DeleteAsync(task, cancellationToken);
    }

    public async Task<TaskResponse?> GetTaskByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetTaskWithDetailsAsync(id, cancellationToken);
        if (task == null)
            return null;

        return MapToTaskResponse(task);
    }

    public async Task<PaginatedResult<TaskResponse>> GetTasksAsync(
        int pageNumber, 
        int pageSize, 
        string? status = null, 
        string? priority = null, 
        CancellationToken cancellationToken = default)
    {
        var tasks = await _taskRepository.GetTasksWithDetailsAsync(pageNumber, pageSize, status, priority, cancellationToken);
        var totalCount = await _taskRepository.CountAsync(
            t => (string.IsNullOrEmpty(status) || t.Status.ToString() == status) && 
                 (string.IsNullOrEmpty(priority) || t.Priority.ToString() == priority), 
            cancellationToken);

        var items = tasks.Select(MapToTaskResponse).ToList();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PaginatedResult<TaskResponse>(items, totalCount, pageNumber, pageSize, totalPages);
    }

    public async Task<List<TaskResponse>> GetMyTasksAsync(int userId, CancellationToken cancellationToken = default)
    {
        var tasks = await _taskRepository.GetTasksByUserIdAsync(userId, cancellationToken);
        return tasks.Select(MapToTaskResponse).ToList();
    }

    private static TaskResponse MapToTaskResponse(TaskItem task)
    {
        return new TaskResponse(
            task.Id,
            task.Title,
            task.Description,
            task.Status,
            task.Priority,
            task.Deadline,
            task.ProjectId,
            task.Project.Name,
            task.TaskAssignments.Select(ta => new AssignedUserDto(
                ta.UserId,
                ta.User.Email,
                $"{ta.User.FirstName} {ta.User.LastName}"
            )).ToList(),
            task.CreatedAt
        );
    }
}
