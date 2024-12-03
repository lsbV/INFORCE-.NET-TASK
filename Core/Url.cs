namespace Core;

public record Url(
    OriginalUrl OriginalUrl,
    UrlHash Hash,
    UrlExpiration Expiration,
    UrlVisits Visits,
    UserId CreatedBy,
    UrlCreationTime CreatedAt
    );


public record OriginalUrl(string Value);


public record UrlHash(string Value);


public record UrlExpiration(DateTime Value);


public record UrlVisits(uint Value)
{
    public static UrlVisits Empty => new(0);
}


public record UrlCreationTime(DateTime Value)
{
    public static UrlCreationTime Now => new(DateTime.Now);
}

public record UrlInfo(
    UrlExpiration Expiration,
    UrlVisits Visits,
    UserId CreatedBy,
    UrlCreationTime CreatedAt
    );