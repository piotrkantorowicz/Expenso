namespace Expenso.DocumentManagement.Core.Application.Shared.Services;

internal interface IDirectoryInfoService
{
    string GetReportsDirectory(string userId, string[]? groups, string date);

    string GetImportsDirectory(string userId, string[]? groups, string date);
}