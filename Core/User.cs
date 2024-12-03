using Microsoft.AspNetCore.Identity;

namespace Core;

public class User : IdentityUser<UserId>;

public record UserId(int Value);