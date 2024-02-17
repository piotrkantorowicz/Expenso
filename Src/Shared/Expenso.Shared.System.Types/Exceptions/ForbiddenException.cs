namespace Expenso.Shared.System.Types.Exceptions;

public sealed class ForbiddenException(string message) : Exception(message);