using UserComponent;

namespace Server.Behavior;

public class AddCreatorEmailBehavior(IUserService service)
    : IPipelineBehavior<GetUrlRequest, Url>
{
    public async Task<Url> Handle(GetUrlRequest request, RequestHandlerDelegate<Url> next, CancellationToken cancellationToken)
    {
        var url = await next();
        var creator = await service.GetUserByIdAsync(url.CreatedBy);
        return new UrlWithCreatorEmail(
            url.OriginalUrl,
            url.Hash,
            url.Expiration,
            url.Visits,
            url.CreatedBy,
            url.CreatedAt,
            new Email(creator.Email!)
        );
    }
}