namespace Server.Exceptions;

public class UnauthorizedException(string identifier)
    : Exception($"Unauthorized request with identifier {identifier}");