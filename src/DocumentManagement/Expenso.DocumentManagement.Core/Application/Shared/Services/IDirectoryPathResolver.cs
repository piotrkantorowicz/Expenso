using Expenso.DocumentManagement.Core.Application.Shared.Models;

namespace Expenso.DocumentManagement.Core.Application.Shared.Services;

internal interface IDirectoryPathResolver
{
    string ResolvePath(FileType fileType, string userId, string[]? groups);
}