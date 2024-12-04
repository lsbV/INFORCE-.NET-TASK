namespace ShortenerComponent.Operations;

public record VisitRequest(UrlHash UrlHash) : IRequest<Url>;

internal class VisitUrlHandler(ApplicationDbContext context)
    : IRequestHandler<VisitRequest, Url>
{
    public async Task<Url> Handle(VisitRequest request, CancellationToken cancellationToken)
    {
        var url = await context.Urls.AsNoTracking().FirstOrDefaultAsync(u => u.Hash == request.UrlHash, cancellationToken);
        if (url is null)
        {
            throw new UrlNotFoundException(request.UrlHash);
        }
        url = url with { Visits = (UrlVisits.Increment(url.Visits)) };
        context.Update(url);
        await context.SaveChangesAsync(cancellationToken);
        return url;
    }
}