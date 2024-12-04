namespace Server.Dtos;

public record EntireUrlDto(string OriginalUrl, string ShortenedUrl, DateTime Expiration, uint Visits, string CreatedBy, DateTime CreatedAt);