namespace Expenso.Shared.Types.Exceptions;

public sealed class ConflictException(string message) : Exception(message);