using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

internal sealed class EmptyFileContentException() : ValidationException("File content cannot be empty.");