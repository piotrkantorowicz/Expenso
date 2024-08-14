namespace Expenso.Shared.System.Types.Exceptions;

public sealed class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message: message)
    {
    }
}