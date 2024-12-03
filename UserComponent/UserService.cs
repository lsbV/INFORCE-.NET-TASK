using Core;
using Microsoft.AspNetCore.Identity;

namespace UserComponent;

public class UserService(UserManager<User> userManager, ITokenService tokenService) : IUserService
{

    public async Task<IdentityResult> RegisterUserAsync(RegisterRequest model)
    {
        var user = new User { UserName = model.Email, Email = model.Email };
        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "User");
        }
        return result;
    }

    public async Task<string?> AuthenticateUserAsync(LoginRequest model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(user);
        return tokenService.GenerateJwtToken(user, roles.Single());
    }
}