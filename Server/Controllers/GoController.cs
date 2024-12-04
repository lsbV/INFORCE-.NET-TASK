namespace Server.Controllers;

[Route("[controller]")]
[Controller]
public class GoController(ISender sender) : ControllerBase
{
    [HttpGet("{url}")]
    public async Task<IActionResult> Get(string url)
    {
        var destination = await sender.Send(new VisitRequest(new UrlHash(url)));
        return Redirect(destination.OriginalUrl.Value);
    }
}

