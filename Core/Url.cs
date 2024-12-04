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


public record UrlExpiration(DateTime Value)
{
    public static bool operator >(UrlExpiration left, DateTime right) => left.Value > right;
    public static bool operator <(UrlExpiration left, DateTime right) => left.Value < right;
}


public record UrlVisits(uint Value)
{
    public static UrlVisits Empty => new(0);

    public static UrlVisits Increment(UrlVisits visits) => new(visits.Value + 1);
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