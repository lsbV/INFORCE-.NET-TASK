using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using UserComponent;

namespace Tests.UserComponent;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;
    private readonly SigningCredentials _signingCredentials;

    public TokenServiceTests()
    {
        var key = new SymmetricSecurityKey("supersecretkey123456789012345678"u8.ToArray());
        _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        _tokenService = new TokenService(_signingCredentials);
    }

    [Fact]
    public void GenerateJwtToken_ShouldReturnValidToken()
    {
        // Arrange
        var user = new User
        {
            Id = new UserId(1),
            UserName = "testUser",
            Email = "testuser@example.com"
        };
        var role = "Admin";

        // Act
        var token = _tokenService.GenerateJwtToken(user, role);

        // Assert
        Assert.NotNull(token);
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _signingCredentials.Key,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        Assert.NotNull(validatedToken);
        Assert.Equal(user.Id.Value.ToString(), principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Assert.Equal(user.UserName, principal.FindFirst(ClaimTypes.Name)?.Value);
        Assert.Equal(user.Email, principal.FindFirst(ClaimTypes.Email)?.Value);
        Assert.Equal(role, principal.FindFirst(ClaimTypes.Role)?.Value);
    }
}
