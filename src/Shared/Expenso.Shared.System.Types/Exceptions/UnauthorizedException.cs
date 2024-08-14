namespace Expenso.Shared.System.Types.Exceptions;

public sealed class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message: message)
    {
    }
}