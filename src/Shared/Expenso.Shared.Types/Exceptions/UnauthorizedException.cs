namespace Expenso.Shared.Types.Exceptions;

public sealed class UnauthorizedException(string message) : Exception(message);