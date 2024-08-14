namespace Expenso.Shared.System.Types.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message: message)
    {
    }
}