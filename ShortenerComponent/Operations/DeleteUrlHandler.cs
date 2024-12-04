namespace ShortenerComponent.Operations;

public record DeleteUrlCommand(UrlHash UrlHash, UserId UserId) : IRequest<Url>;

public class DeleteUrlHandler(ApplicationDbContext context)
    : IRequestHandler<DeleteUrlCommand, Url>
{
    public async Task<Url> Handle(DeleteUrlCommand request, CancellationToken cancellationToken)
    {
        var url = await context.Urls.FindAsync([request.UrlHash], cancellationToken);
        if (url is null)
        {
            throw new UrlNotFoundException(request.UrlHash);
        }
        context.Remove(url);
        await context.SaveChangesAsync(cancellationToken);
        return url;

    }
}