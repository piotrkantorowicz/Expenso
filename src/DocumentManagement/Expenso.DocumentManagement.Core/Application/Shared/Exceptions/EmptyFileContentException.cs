using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Validation;

namespace Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

internal sealed class EmptyFileContentException : ValidationException
{
    public EmptyFileContentException() : base(details: "File content cannot be empty")
    {
    }
}