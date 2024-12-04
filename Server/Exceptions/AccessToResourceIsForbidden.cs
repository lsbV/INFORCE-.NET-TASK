namespace Server.Exceptions;

public class AccessToResourceIsForbidden(UserId Requester, string resourceId) : Exception($"User {Requester} does not have access to resource {resourceId}");