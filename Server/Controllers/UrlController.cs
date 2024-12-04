namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UrlController(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpGet("{url}")]
    public async Task<IActionResult> Get(string url)
    {
        var result = await sender.Send(new GetUrlRequest(new UrlHash(url), this.GetUserId()));
        var responseDto = result.ToEntireDto();
        return Ok(responseDto);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationInfo pagination)
    {
        var result = await sender.Send(new GetUrlsRequest(pagination, null));
        var responseDtos = result.Select(x => x.ToShortDto());
        return Ok(responseDtos);
    }

    [HttpGet("user/{userId:int}")]
    [Authorize]
    public async Task<IActionResult> GetForUser([FromQuery] PaginationInfo pagination, int userId)
    {
        var result = await sender.Send(new GetUrlsRequest(pagination,
            new GetUrlsRequestAdditionInfo(this.GetUserId(), new UserId(userId))));
        var responseDtos = result.Select(x => x.ToShortDto());
        return Ok(responseDtos);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] CreateShortUrlRequest request)
    {
        var result = await sender.Send(new ShortenUrlCommand(new OriginalUrl(request.Url), this.GetUserId()));
        var responseDto = result.ToEntireDto();
        return CreatedAtAction(nameof(Get), new { url = responseDto.ShortenedUrl }, responseDto);
    }

    [HttpDelete("{url}")]
    [Authorize]
    public async Task<IActionResult> Delete(string url)
    {
        var result = await sender.Send(new DeleteUrlCommand(new UrlHash(url), this.GetUserId()));
        var responseDto = result.ToEntireDto();
        return Ok(responseDto);
    }
}