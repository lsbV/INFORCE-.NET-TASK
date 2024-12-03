namespace ShortenerComponent.Exceptions;

public class ForbiddenOperationException(UserId userId, string resource) : Exception($"User {userId} has no access to {resource}");