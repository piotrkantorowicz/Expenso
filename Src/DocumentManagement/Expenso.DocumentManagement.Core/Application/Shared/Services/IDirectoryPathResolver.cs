namespace Expenso.DocumentManagement.Core.Application.Shared.Services;

internal interface IDirectoryPathResolver
{
    string ResolvePath(int fileType, string userId, string[]? groups);
}