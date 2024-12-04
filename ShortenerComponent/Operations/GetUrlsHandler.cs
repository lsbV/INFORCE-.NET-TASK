namespace ShortenerComponent.Operations;

public record GetUrlsRequest(PaginationInfo Pagination, GetUrlsRequestAdditionInfo? AdditionInfo) : IRequest<List<Url>>;
public record GetUrlsRequestAdditionInfo(UserId Requester, UserId About);


internal class GetUrlsHandler(ApplicationDbContext context)
    : IRequestHandler<GetUrlsRequest, List<Url>>
{
    public async Task<List<Url>> Handle(GetUrlsRequest request, CancellationToken cancellationToken)
    {
        var page = request.Pagination.Page > 0 ? (request.Pagination.Page - 1) : 0;
        var urls = request.AdditionInfo is not null
            ? await context.Urls
                .Where(x => x.CreatedBy == request.AdditionInfo.About)
                .OrderBy(x => x.CreatedAt)
                .Skip(page * request.Pagination.Count)
                .Take(request.Pagination.Count)
                .ToListAsync(cancellationToken)

            : await context.Urls
                .OrderBy(x => x.CreatedAt)
                .Skip(page * request.Pagination.Count)
                .Take(request.Pagination.Count)
                .ToListAsync(cancellationToken);
        return urls;
    }
}