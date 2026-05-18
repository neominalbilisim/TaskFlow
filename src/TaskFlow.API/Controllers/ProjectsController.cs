using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectResponse>>> GetProjects(CancellationToken cancellationToken)
    {
        var result = await _projectService.GetAllProjectsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectResponse>> GetProject(int id, CancellationToken cancellationToken)
    {
        var result = await _projectService.GetProjectByIdAsync(id, cancellationToken);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("{id}/tasks")]
    public async Task<ActionResult<List<TaskResponse>>> GetProjectTasks(int id, CancellationToken cancellationToken)
    {
        var result = await _projectService.GetProjectTasksAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<ProjectResponse>> CreateProject(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var result = await _projectService.CreateProjectAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetProject), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<ProjectResponse>> UpdateProject(int id, UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var result = await _projectService.UpdateProjectAsync(id, request, cancellationToken);
        return Ok(result);
    }
}
