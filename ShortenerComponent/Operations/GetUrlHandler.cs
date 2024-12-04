namespace ShortenerComponent.Operations;

public record GetUrlRequest(UrlHash UrlHash, UserId Requester) : IRequest<Url>;

public class GetUrlHandler(ApplicationDbContext context)
    : IRequestHandler<GetUrlRequest, Url>
{
    public async Task<Url> Handle(GetUrlRequest request, CancellationToken cancellationToken)
    {
        var url = await context.Urls.FindAsync([request.UrlHash], cancellationToken: cancellationToken);
        if (url is null)
        {
            throw new UrlNotFoundException(request.UrlHash);
        }
        return url;
    }
}