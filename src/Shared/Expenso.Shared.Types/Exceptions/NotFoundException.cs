namespace Expenso.Shared.Types.Exceptions;

public sealed class NotFoundException(string message) : Exception(message);