using Microsoft.Extensions.Options;

namespace Server.Behavior;

public class ConcatBaseUrlBehavior(IOptions<ConcatBaseUrlBehaviorOptions> options)
    : IPipelineBehavior<GetUrlRequest, Url>,
        IPipelineBehavior<GetUrlsRequest, List<Url>>
{
    private readonly ConcatBaseUrlBehaviorOptions _options = options.Value;

    public async Task<Url> Handle(GetUrlRequest request, RequestHandlerDelegate<Url> next, CancellationToken cancellationToken)
    {
        var url = await next();
        return url with { Hash = new UrlHash(_options.BaseUrl + url.Hash.Value) };
    }

    public async Task<List<Url>> Handle(GetUrlsRequest request, RequestHandlerDelegate<List<Url>> next, CancellationToken cancellationToken)
    {
        var urls = await next();
        return urls.Select(url => url with { Hash = new UrlHash(_options.BaseUrl + url.Hash.Value) }).ToList();
    }
}

public class ConcatBaseUrlBehaviorOptions
{
    public required string BaseUrl { get; set; }
}