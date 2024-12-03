using Core;

namespace ShortenerComponent.Exceptions;

public class UrlNotFoundException(UrlHash hash) : Exception($"Url with hash {hash} not found");