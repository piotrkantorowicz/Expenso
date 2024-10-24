using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

internal sealed class EmptyPathException : ValidationException
{
    public EmptyPathException() : base(details: "Path cannot be empty")
    {
    }
}