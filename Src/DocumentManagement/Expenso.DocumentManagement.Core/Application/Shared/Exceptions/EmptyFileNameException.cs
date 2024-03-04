using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

internal sealed class EmptyFileNameException() : ValidationException("File name cannot be empty.");