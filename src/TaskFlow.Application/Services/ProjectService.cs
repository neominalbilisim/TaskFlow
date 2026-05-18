using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var project = new Project
        {
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        await _projectRepository.AddAsync(project, cancellationToken);

        return new ProjectResponse(project.Id, project.Name, project.Description, project.IsActive, 
            project.StartDate, project.EndDate, 0);
    }

    public async Task<ProjectResponse> UpdateProjectAsync(int id, UpdateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdAsync(id, cancellationToken);
        if (project == null)
            throw new NotFoundException("Project not found");

        project.Name = request.Name;
        project.Description = request.Description;
        project.IsActive = request.IsActive;
        project.StartDate = request.StartDate;
        project.EndDate = request.EndDate;

        await _projectRepository.UpdateAsync(project, cancellationToken);

        return new ProjectResponse(project.Id, project.Name, project.Description, project.IsActive,
            project.StartDate, project.EndDate, project.Tasks.Count);
    }

    public async Task<ProjectResponse?> GetProjectByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdAsync(id, cancellationToken);
        if (project == null)
            return null;

        return new ProjectResponse(project.Id, project.Name, project.Description, project.IsActive,
            project.StartDate, project.EndDate, 0);
    }

    public async Task<List<ProjectResponse>> GetAllProjectsAsync(CancellationToken cancellationToken = default)
    {
        var projects = await _projectRepository.GetAllAsync(cancellationToken);
        return projects.Select(p => new ProjectResponse(p.Id, p.Name, p.Description, p.IsActive,
            p.StartDate, p.EndDate, 0)).ToList();
    }

    public async Task<List<TaskResponse>> GetProjectTasksAsync(int projectId, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetProjectWithTasksAsync(projectId, cancellationToken);
        if (project == null)
            throw new NotFoundException("Project not found");

        return project.Tasks.Select(t => new TaskResponse(
            t.Id, t.Title, t.Description, t.Status, t.Priority, t.Deadline, t.ProjectId, project.Name,
            t.TaskAssignments.Select(ta => new AssignedUserDto(ta.UserId, ta.User.Email, 
                $"{ta.User.FirstName} {ta.User.LastName}")).ToList(), t.CreatedAt
        )).ToList();
    }
}
