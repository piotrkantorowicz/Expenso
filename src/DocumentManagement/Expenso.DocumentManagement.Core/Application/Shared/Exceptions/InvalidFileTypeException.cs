using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

internal sealed class InvalidFileTypeException : ValidationException
{
    public InvalidFileTypeException(string typeName) : base(details: $"Invalid file type: {typeName}")
    {
    }
}