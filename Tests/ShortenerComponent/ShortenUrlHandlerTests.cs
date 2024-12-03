using ShortenerComponent.Options;

namespace Tests.ShortenerComponent;

public class ShortenUrlHandlerTests
{
    private readonly ApplicationDbContext _context = TestHelper.GetDbContext();
    private readonly ShortenerServiceOptions _options = TestHelper.GetShortenerServiceOptions();



    [Fact]
    public async Task Handle_ShouldReturnShortenedUrl()
    {
        var command = new ShortenUrlCommand(TestHelper.GetRandomUrl(), new UserId(1));
        var handler = new ShortenUrlHandler(_context, _options);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(_options.HashLength, result.Hash.Value.Length);
    }

    [Fact]
    public async Task Handle_ShouldThrowUrlAlreadyExistsException()
    {
        var handler = new ShortenUrlHandler(_context, _options);
        var duplicatedUrl = TestHelper.GetRandomUrl();
        var url = new Url(
            duplicatedUrl,
            new UrlHash("TEST34"),
            new UrlExpiration(DateTime.Now + _options.DefaultExpirationTime),
            new UrlVisits(2),
            new UserId(1),
            new UrlCreationTime(DateTime.Now));

        _context.Urls.Add(url);
        await _context.SaveChangesAsync();

        var command = new ShortenUrlCommand(duplicatedUrl, new UserId(1));

        await Assert.ThrowsAsync<UrlAlreadyExistsException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowTooMuchAttemptsException()
    {
        _context.Urls.Add(new Url(
            TestHelper.GetRandomUrl(),
            new UrlHash("1"),
            new UrlExpiration(DateTime.Now + _options.DefaultExpirationTime),
            new UrlVisits(2),
            new UserId(1),
            new UrlCreationTime(DateTime.Now)));
        await _context.SaveChangesAsync();

        var command = new ShortenUrlCommand(TestHelper.GetRandomUrl(), new UserId(1));
        var handler = new ShortenUrlHandler(_context, new ShortenerServiceOptions
        {
            HashLength = 1,
            MaxAttempts = 1,
            AllowedCharacters = "1",
            DefaultExpirationTime = TimeSpan.FromDays(1)
        });

        await Assert.ThrowsAsync<TooMuchAttemptsException>(() => handler.Handle(command, CancellationToken.None));
    }

}