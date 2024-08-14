using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

internal sealed class EmptyFileContentException : ValidationException
{
    public EmptyFileContentException() : base(details: "File content cannot be empty")
    {
    }
}