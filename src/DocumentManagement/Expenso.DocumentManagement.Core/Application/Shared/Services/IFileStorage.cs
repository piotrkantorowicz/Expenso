namespace Expenso.DocumentManagement.Core.Application.Shared.Services;

internal interface IFileStorage
{
    Task<byte[]> ReadAsync(string path, CancellationToken cancellationToken);

    Task SaveAsync(string directoryPath, string fileName, byte[] byteContent, CancellationToken cancellationToken);

    Task DeleteAsync(string path, CancellationToken cancellationToken);
}