namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShortenerController(ISender sender) : ControllerBase
{
    [HttpGet("{url}")]
    public async Task<IActionResult> Get(string url)
    {
        var result = await sender.Send(new GetUrlRequest(new UrlHash(url)));
        var responseDto = result.ToDto();
        return Ok(responseDto);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(string url)
    {
        if (!int.TryParse(User.Identity?.Name, out var userId))
        {
            return Unauthorized();
        }

        var result = await sender.Send(new ShortenUrlCommand(new OriginalUrl(url), new UserId(userId)));
        var responseDto = result.ToDto();
        return CreatedAtAction(nameof(Get), new { url = responseDto.ShortenedUrl }, responseDto);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete(Uri url)
    {
        if (!int.TryParse(User.Identity?.Name, out var userId))
        {
            return Unauthorized();
        }

        var result = await sender.Send(new DeleteUrlCommand(new UrlHash(url.AbsolutePath), new UserId(userId)));
        var responseDto = result.ToDto();
        return Ok(responseDto);
    }
}



public record UrlDto(string OriginalUrl, string ShortenedUrl, DateTime Expiration, uint Visits, int CreatedBy, DateTime CreatedAt);



internal static class Extensions
{
    public static UrlDto ToDto(this Url url) => new(
        url.OriginalUrl.Value.ToString(),
        url.Hash.Value,
        url.Expiration.Value,
        url.Visits.Value,
        url.CreatedBy.Value,
        url.CreatedAt.Value);
}