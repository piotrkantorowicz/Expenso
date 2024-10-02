using Expenso.Shared.System.Types.Exceptions.Validation;

namespace Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

internal sealed class EmptyFileNameException : ValidationException
{
    public EmptyFileNameException() : base(details: "File name cannot be empty")
    {
    }
}