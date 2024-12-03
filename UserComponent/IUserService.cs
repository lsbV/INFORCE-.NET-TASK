using Microsoft.AspNetCore.Identity;

namespace UserComponent;

public interface IUserService
{
    Task<IdentityResult> RegisterUserAsync(RegisterRequest model);
    Task<string?> AuthenticateUserAsync(LoginRequest model);
}