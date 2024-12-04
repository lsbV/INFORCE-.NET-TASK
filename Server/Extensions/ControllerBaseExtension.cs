using System.Security.Claims;
using Server.Exceptions;

namespace Server.Extensions;

public static class ControllerBaseExtension
{
    public static UserId GetUserId(this ControllerBase controller)
    {
        var idString = controller.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(idString, out var id))
        {
            throw new UnauthorizedException(controller.HttpContext.TraceIdentifier);
        }

        return new UserId(id);
    }

}