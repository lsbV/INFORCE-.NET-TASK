using Core;

namespace UserComponent;

public interface ITokenService
{
    string GenerateJwtToken(User user, string role);
}