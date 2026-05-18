using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
            return null;

        return new UserResponse(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive);
    }

    public async Task<List<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(u => new UserResponse(u.Id, u.Email, u.FirstName, u.LastName, u.Role, u.IsActive)).ToList();
    }

    public async Task<UserResponse> UpdateUserRoleAsync(int id, UpdateUserRoleRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
            throw new NotFoundException("User not found");

        user.Role = request.Role;
        await _userRepository.UpdateAsync(user, cancellationToken);

        return new UserResponse(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive);
    }
}
