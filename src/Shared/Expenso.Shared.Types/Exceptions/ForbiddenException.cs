namespace Expenso.Shared.Types.Exceptions;

public sealed class ForbiddenException(string message) : Exception(message);