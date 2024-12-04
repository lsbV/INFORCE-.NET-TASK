namespace Tests.ShortenerComponent;

public class DeleteUrlHandlerTests
{
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly DeleteUrlHandler _handler;

    public DeleteUrlHandlerTests()
    {
        _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
        _handler = new DeleteUrlHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_UrlNotFound_ThrowsUrlNotFoundException()
    {
        var command = new DeleteUrlCommand(new UrlHash("testHash"), new UserId(1));
        _mockContext.Setup(c => c.Urls.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Url)null);

        await Assert.ThrowsAsync<UrlNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    // TODO: rewrite to test AuthorityBehavior 
    //[Fact]
    public async Task Handle_UserNotOwner_ThrowsForbiddenOperationException()
    {
        var url = new Url(
            TestHelper.GetRandomUrl(),
            new UrlHash("QWERTY"),
            new UrlExpiration(DateTime.Now + TimeSpan.FromDays(1)),
            new UrlVisits(1),
            new UserId(1),
            new UrlCreationTime(DateTime.Now));
        var command = new DeleteUrlCommand(new UrlHash("testHash"), new UserId(2));
        _mockContext.Setup(c => c.Urls.FindAsync(It.IsAny<object[]>(), CancellationToken.None))
                    .ReturnsAsync(url);

        await Assert.ThrowsAsync<ForbiddenOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesUrl()
    {
        var url = new Url(
            TestHelper.GetRandomUrl(),
            new UrlHash("testHash"),
            new UrlExpiration(DateTime.Now + TimeSpan.FromDays(1)),
            new UrlVisits(1),
            new UserId(2),
            new UrlCreationTime(DateTime.Now));
        var command = new DeleteUrlCommand(new UrlHash("testHash"), new UserId(2));
        _mockContext.Setup(c => c.Urls.FindAsync(It.IsAny<object[]>(), CancellationToken.None))
                    .ReturnsAsync(url);
        _mockContext.Setup(c => c.Remove(It.IsAny<Url>()));
        _mockContext.Setup(c => c.SaveChangesAsync(CancellationToken.None))
                    .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(url, result);
        _mockContext.Verify(c => c.Remove(url), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
