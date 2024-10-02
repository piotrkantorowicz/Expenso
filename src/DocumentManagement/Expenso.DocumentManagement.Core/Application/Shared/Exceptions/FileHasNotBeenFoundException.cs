using Expenso.Shared.System.Types.Exceptions.Validation;

namespace Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

internal sealed class FileHasNotBeenFoundException : ValidationException
{
    public FileHasNotBeenFoundException() : base(details: "File not found")
    {
    }
}