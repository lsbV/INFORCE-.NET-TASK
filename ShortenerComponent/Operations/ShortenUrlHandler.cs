namespace ShortenerComponent.Operations;

public record ShortenUrlCommand(OriginalUrl OriginalUrl, UserId UserId) : IRequest<Url>;

public class ShortenUrlHandler(ApplicationDbContext context, ShortenerServiceOptions options)
    : IRequestHandler<ShortenUrlCommand, Url>
{
    public async Task<Url> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        if (await context.Urls.AnyAsync(u => u.OriginalUrl == request.OriginalUrl, cancellationToken))
        {
            throw new UrlAlreadyExistsException(request.OriginalUrl);
        }
        var hash = await GenerateHash();

        var url = new Url(
            request.OriginalUrl,
            hash,
            new UrlExpiration(DateTime.Now + options.DefaultExpirationTime),
            UrlVisits.Empty,
            request.UserId,
            UrlCreationTime.Now
        );

        context.Add(url);
        await context.SaveChangesAsync(cancellationToken);
        return url;
    }



    private async Task<UrlHash> GenerateHash()
    {
        var attempts = 0;
        while (attempts < options.MaxAttempts)
        {
            var hash = GenerateRandomHash();
            if (!await context.Urls.AnyAsync(u => u.Hash == hash))
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
        for (var i = 0; i < options.HashLength; i++)
        {
            var index = random.Next(0, options.AllowedCharacters.Length);
            hash.Append(options.AllowedCharacters[index]);
        }

        return new UrlHash(hash.ToString());
    }

}
