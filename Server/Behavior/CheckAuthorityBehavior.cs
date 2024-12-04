using Azure.Core;
using Server.Exceptions;
using UserComponent;

namespace Server.Behavior;

public class CheckAuthorityBehavior(IUserService service)
    : IPipelineBehavior<GetUrlRequest, Url>,
        IPipelineBehavior<DeleteUrlCommand, Url>

{
    public async Task<Url> Handle(GetUrlRequest request, RequestHandlerDelegate<Url> next, CancellationToken cancellationToken)
    {
        var url = await next();
        if (await UserHasAccessToUrl(request.Requester, url))
        {
            return url;
        }

        throw new AccessToResourceIsForbidden(request.Requester, url.Hash.Value);
    }

    public async Task<Url> Handle(DeleteUrlCommand request, RequestHandlerDelegate<Url> next, CancellationToken cancellationToken)
    {
        var url = await next();
        if (await UserHasAccessToUrl(request.UserId, url))
        {
            return url;
        }
        throw new AccessToResourceIsForbidden(request.UserId, url.Hash.Value);
    }

    private async Task<bool> UserHasAccessToUrl(UserId userId, Url url)
    {
        var user = await service.GetUserByIdAsync(userId);
        var role = await service.GetRoleAsyncByUserId(user);
        return role.Name == "Admin" || userId == url.CreatedBy;
    }
}