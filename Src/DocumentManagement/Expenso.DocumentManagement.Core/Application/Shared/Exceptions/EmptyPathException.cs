using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

internal sealed class EmptyPathException() : ValidationException("Path cannot be empty.");