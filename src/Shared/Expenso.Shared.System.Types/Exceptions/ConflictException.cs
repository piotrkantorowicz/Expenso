namespace Expenso.Shared.System.Types.Exceptions;

public sealed class ConflictException : Exception
{
    public ConflictException(string message) : base(message: message)
    {
    }
}