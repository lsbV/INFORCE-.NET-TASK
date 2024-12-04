namespace Server.Dtos;

public record UrlWithCreatorEmail(
    OriginalUrl OriginalUrl,
    UrlHash Hash,
    UrlExpiration Expiration,
    UrlVisits Visits,
    UserId CreatedBy,
    UrlCreationTime CreatedAt,
    Email CreatorEmail)
    : Url(OriginalUrl, Hash, Expiration, Visits, CreatedBy, CreatedAt);

public record Email(string Value);