using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetUsers(CancellationToken cancellationToken)
    {
        var result = await _userService.GetAllUsersAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetUser(int id, CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserByIdAsync(id, cancellationToken);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{id}/role")]
    public async Task<ActionResult<UserResponse>> UpdateUserRole(int id, UpdateUserRoleRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateUserRoleAsync(id, request, cancellationToken);
        return Ok(result);
    }
}
