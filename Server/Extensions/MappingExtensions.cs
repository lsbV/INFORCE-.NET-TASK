namespace Server.Extensions;

public static class MappingExtensions
{
    public static EntireUrlDto ToEntireDto(this Url url)
    {
        if (url is UrlWithCreatorEmail urlWithCreator)
        {
            return new EntireUrlDto(
                url.OriginalUrl.Value,
                url.Hash.Value,
                url.Expiration.Value,
                url.Visits.Value,
                urlWithCreator.CreatorEmail.Value,
                url.CreatedAt.Value);
        }
        return new EntireUrlDto(
            url.OriginalUrl.Value,
            url.Hash.Value,
            url.Expiration.Value,
            url.Visits.Value,
            url.CreatedBy.Value.ToString(),
            url.CreatedAt.Value);
    }

    public static ShortUrlDto ToShortDto(this Url url) => new(
         url.OriginalUrl.Value,
        url.Hash.Value);
}