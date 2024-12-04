using Core;
using Database;
using Microsoft.AspNetCore.Identity;

namespace UserComponent;

public class UserService(UserManager<User> userManager, ApplicationDbContext context, ITokenService tokenService) : IUserService
{

    public async Task<IdentityResult> RegisterUserAsync(RegisterRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user != null) return IdentityResult.Failed(new IdentityError { Description = "User already exists." });

        user = new User { UserName = request.Email, Email = request.Email };
        var result = await userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "User");
        }
        return result;
    }

    public async Task<LoginResult?> AuthenticateUserAsync(LoginRequest model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = tokenService.GenerateJwtToken(user, roles.Single());
        var loginResult = new LoginResult(token, user.Email!, roles.Single(), user.Id.Value);
        return loginResult;


    }

    public async Task<User> GetUserByIdAsync(UserId userId)
    {
        var user = await context.Users.FindAsync(userId);
        return user!;
    }

    public async Task<Role> GetRoleAsyncByUserId(User user)
    {
        var roles = await userManager.GetRolesAsync(user);
        return new Role() { Name = roles.Single() };
    }
}