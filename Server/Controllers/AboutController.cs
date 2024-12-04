using System.Text.Encodings.Web;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AboutController : ControllerBase
{
    private string _about = """
                            Summary of URL Shortening Algorithm
                            1.	Shorten URL Command:
                            •	The user submits a ShortenUrlCommand with the original URL and user ID.
                            2.	ShortenUrlHandler:
                            •	Handle Method:
                            •	Checks if the original URL already exists in the database.
                            •	If it exists, throws a UrlAlreadyExistsException.
                            •	If not, generates a unique hash using the GenerateHash method.
                            •	Creates a new Url object with the original URL, generated hash, expiration time, visit count, user ID, and creation time.
                            •	Adds the new URL to the database and saves the changes.
                            •	Returns the newly created Url object.
                            3.	GenerateHash Method:
                            •	Attempts to generate a unique hash up to a maximum number of attempts.
                            •	Uses the GenerateRandomHash method to create a random hash.
                            •	Checks if the generated hash already exists in the database.
                            •	If a unique hash is found, returns it.
                            •	If the maximum attempts are reached without finding a unique hash, throws a TooMuchAttemptsException.
                            4.	GenerateRandomHash Method:
                            •	Generates a random hash string using allowed characters and specified hash length.
                            •	Returns the generated hash as a UrlHash object.
                            Behaviors Involved:
                            •	AddCreatorEmailBehavior: Adds the creator's email to the URL metadata.
                            •	ConcatBaseUrlBehavior: Concatenates the base URL with the shortened hash to form the final shortened URL.
                            •	CheckAuthorityBehavior: Ensures the user has the authority to shorten the URL.
                            •	DistributedCacheBehavior: Caches the URL and its visit count to improve performance and reduce database load.
                            
                            """;

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