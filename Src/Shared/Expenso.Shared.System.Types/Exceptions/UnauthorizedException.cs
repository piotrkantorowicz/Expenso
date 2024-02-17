namespace Expenso.Shared.System.Types.Exceptions;

public sealed class UnauthorizedException(string message) : Exception(message);