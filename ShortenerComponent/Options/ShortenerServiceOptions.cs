namespace ShortenerComponent.Options;

public class ShortenerServiceOptions
{
    public int HashLength { get; set; }
    public int MaxAttempts { get; set; }
    public string AllowedCharacters { get; set; }
    public TimeSpan DefaultExpirationTime { get; set; }
}