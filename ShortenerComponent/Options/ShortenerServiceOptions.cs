namespace ShortenerComponent.Options;

public record ShortenerServiceOptions(
    DateTime DefaultExpirationTime,
    char[] AllowedCharacters,
    int HashLength,
    int MaxAttempts
);