namespace Tests.ShortenerComponent;

public class GetUrlHandlerTests
{
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly GetUrlHandler _handler;

    public GetUrlHandlerTests()
    {
        _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
        _handler = new GetUrlHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_UrlExists_ReturnsUrl()
    {
        var urlHash = new UrlHash("testHash");
        var url = new Url(
            new OriginalUrl("http://example.com"),
            urlHash,
            new UrlExpiration(DateTime.Now + TimeSpan.FromDays(1)),
            UrlVisits.Empty,
            new UserId(1),
            UrlCreationTime.Now
        );
        _mockContext.Setup(c => c.Urls.FindAsync(new object[] { urlHash }, CancellationToken.None))
            .ReturnsAsync(url);
        var request = new GetUrlRequest(urlHash);


        var result = await _handler.Handle(request, CancellationToken.None);


        Assert.NotNull(result);
        Assert.Equal(urlHash, result.Hash);
        Assert.Equal("http://example.com", result.OriginalUrl.Value);
    }

    [Fact]
    public async Task Handle_UrlDoesNotExist_ThrowsUrlNotFoundException()
    {
        var urlHash = new UrlHash("nonExistentHash");
        _mockContext.Setup(c => c.Urls.FindAsync(new object[] { urlHash }, CancellationToken.None))
            .ReturnsAsync((Url)null!);
        var request = new GetUrlRequest(urlHash);


        await Assert.ThrowsAsync<UrlNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }
}