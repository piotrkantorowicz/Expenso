using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

internal sealed class EmptyFileNameException : ValidationException
{
    public EmptyFileNameException() : base(details: "File name cannot be empty")
    {
    }
}