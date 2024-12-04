using Microsoft.Extensions.Options;

namespace ShortenerComponent.Operations;

public record ShortenUrlCommand(OriginalUrl OriginalUrl, UserId UserId) : IRequest<Url>;

public class ShortenUrlHandler(ApplicationDbContext context, IOptions<ShortenerServiceOptions> options)
    : IRequestHandler<ShortenUrlCommand, Url>
{
    private readonly ShortenerServiceOptions _options = options.Value;
    public async Task<Url> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        if (await context.Urls.AnyAsync(u => u.OriginalUrl == request.OriginalUrl, cancellationToken))
        {
            throw new UrlAlreadyExistsException(request.OriginalUrl);
        }
        var hash = await GenerateHash(cancellationToken);

        var url = new Url(
            request.OriginalUrl,
            hash,
            new UrlExpiration(DateTime.Now + _options.DefaultExpirationTime),
            UrlVisits.Empty,
            request.UserId,
            UrlCreationTime.Now
        );

        context.Add(url);
        await context.SaveChangesAsync(cancellationToken);
        return url;
    }



    private async Task<UrlHash> GenerateHash(CancellationToken cancellationToken)
    {
        var attempts = 0;
        while (attempts < _options.MaxAttempts)
        {
            var hash = GenerateRandomHash();
            if (!await context.Urls.AnyAsync(u => u.Hash == hash, cancellationToken))
            {
                return hash;
            }

            attempts++;
        }

        throw new TooMuchAttemptsException("Failed to generate unique hash");
    }

    private UrlHash GenerateRandomHash()
    {
        var hash = new StringBuilder();
        var random = new Random();
        for (var i = 0; i < _options.HashLength; i++)
        {
            var index = random.Next(0, _options.AllowedCharacters.Length);
            hash.Append(_options.AllowedCharacters[index]);
        }

        return new UrlHash(hash.ToString());
    }

}
