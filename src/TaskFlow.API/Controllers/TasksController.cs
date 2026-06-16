using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Enums;

namespace TaskFlow.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<TaskResponse>>> GetTasks(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        [FromQuery] string? priority = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _taskService.GetTasksAsync(pageNumber, pageSize, status, priority, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponse>> GetTask(int id, CancellationToken cancellationToken)
    {
        var result = await _taskService.GetTaskByIdAsync(id, cancellationToken);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("assigned-to-me")]
    public async Task<ActionResult<List<TaskResponse>>> GetMyTasks(CancellationToken cancellationToken)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _taskService.GetMyTasksAsync(userId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TaskResponse>> CreateTask(CreateTaskRequest request, CancellationToken cancellationToken)
    {

    // CreateTaskRequest cr =  request with
    //    {
    //        Title = request.Title.Trim(),
    //        Description = request.Description?.Trim()
    //    }
    //; 

    var result = await _taskService.CreateTaskAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetTask), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskResponse>> UpdateTask(int id, UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var result = await _taskService.UpdateTaskAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<TaskResponse>> UpdateTaskStatus(int id, UpdateTaskStatusRequest request, CancellationToken cancellationToken)
    {
        var result = await _taskService.UpdateTaskStatusAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> DeleteTask(int id, CancellationToken cancellationToken)
    {
        await _taskService.DeleteTaskAsync(id, cancellationToken);
        return NoContent();
    }
}
