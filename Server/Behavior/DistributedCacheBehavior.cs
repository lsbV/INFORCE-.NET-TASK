using System.Text.Json;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Server.Behavior
{
    public class DistributedCacheBehavior(IDistributedCache cache, ApplicationDbContext context)
        : IPipelineBehavior<VisitRequest, Url>,
                IPipelineBehavior<GetUrlRequest, Url>
    {
        public async Task<Url> Handle(VisitRequest request, RequestHandlerDelegate<Url> next, CancellationToken cancellationToken)
        {
            var urlStr = await cache.GetStringAsync(request.UrlHash.Value, cancellationToken);
            if (urlStr is null)
            {
                var url = await next();
                var t1 = cache.SetStringAsync(request.UrlHash.Value, url.OriginalUrl.Value, cancellationToken);
                var t2 = cache.SetStringAsync(request.UrlHash.Value + ".visits", url.Visits.Value.ToString(), cancellationToken);
                await Task.WhenAll(t1, t2);
                return url;
            }

            var visits = uint.Parse(await cache.GetStringAsync(request.UrlHash.Value + ".visits", cancellationToken) ?? "0") + 1;
            if (visits % 10 == 0)
            {
                var url = await context.Urls.AsNoTracking().FirstOrDefaultAsync(u => u.Hash == request.UrlHash, cancellationToken);
                if (url is not null)
                {
                    url = url with { Visits = new UrlVisits(visits) };
                    context.Update(url);
                    await context.SaveChangesAsync(cancellationToken);
                }

            }
            await cache.SetStringAsync(request.UrlHash.Value + ".visits", visits.ToString(), cancellationToken);

            return new Url(
                new OriginalUrl(urlStr),
                new UrlHash(request.UrlHash.Value),
                new UrlExpiration(DateTime.MaxValue),
                new UrlVisits(visits),
                new UserId(0),
                new UrlCreationTime(DateTime.MinValue)
            );

        }

        public async Task<Url> Handle(GetUrlRequest request, RequestHandlerDelegate<Url> next, CancellationToken cancellationToken)
        {
            var url = await next();
            var visitsFromCache = await cache.GetStringAsync(url.Hash.Value + ".visits", cancellationToken);
            if (visitsFromCache is not null)
            {
                url = url with { Visits = new UrlVisits(uint.Parse(visitsFromCache)) };
            }
            return url;

        }
    }
}