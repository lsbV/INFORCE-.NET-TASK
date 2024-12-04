using System.Text.Encodings.Web;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AboutController : ControllerBase
{
    private string _about = "This is a simple URL shortener service";

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { text = _about });
    }


    [HttpPut]
    [Authorize(Roles = "Admin")]
    public IActionResult Post([FromBody] UpdateAboutRequest request)
    {
        _about = HtmlEncoder.Default.Encode(request.Text);
        return Ok(new { message = "About text updated successfully" });
    }

    public record UpdateAboutRequest(string Text);
}