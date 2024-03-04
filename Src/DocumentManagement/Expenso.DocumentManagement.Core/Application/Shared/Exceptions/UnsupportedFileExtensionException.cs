using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

internal sealed class UnsupportedFileExtensionException(string? extension)
    : ValidationException($"File extension {extension} is not supported.");