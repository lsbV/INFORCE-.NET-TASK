using Core;
using Microsoft.AspNetCore.Identity;

namespace UserComponent;

public interface IUserService
{
    Task<IdentityResult> RegisterUserAsync(RegisterRequest request);
    Task<LoginResult?> AuthenticateUserAsync(LoginRequest model);
    Task<User> GetUserByIdAsync(UserId userId);
    Task<Role> GetRoleAsyncByUserId(User user);
}