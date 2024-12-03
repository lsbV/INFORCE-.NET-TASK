using Core;

namespace ShortenerComponent.Exceptions;

public class UrlAlreadyExistsException(OriginalUrl originalUrl) : Exception($"Url {originalUrl} already exists");