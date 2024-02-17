namespace Expenso.Shared.System.Types.Exceptions;

public sealed class NotFoundException(string message) : Exception(message);