namespace Expenso.Shared.System.Types.Exceptions;

public sealed class ConflictException(string message) : Exception(message: message);