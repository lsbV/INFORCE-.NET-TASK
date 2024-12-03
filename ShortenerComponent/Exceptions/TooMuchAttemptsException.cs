namespace ShortenerComponent.Exceptions;

public class TooMuchAttemptsException(string? message) : Exception(message);