using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

namespace Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk;

internal sealed class FileStorage(IFileSystem fileSystem) : IFileStorage
{
    private readonly IFileSystem _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));

    public async Task<byte[]> ReadAsync(string path, CancellationToken cancellationToken)
    {
        if (!_fileSystem.File.Exists(path))
        {
            throw new FileHasNotBeenFoundException();
        }

        return await _fileSystem.File.ReadAllBytesAsync(path, cancellationToken);
    }

    public async Task SaveAsync(string directoryPath, string fileName, byte[] byteContent,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(directoryPath))
        {
            throw new EmptyPathException();
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new EmptyFileNameException();
        }

        if (byteContent is null || byteContent.Length == 0)
        {
            throw new EmptyFileContentException();
        }

        if (!_fileSystem.Directory.Exists(directoryPath))
        {
            _fileSystem.Directory.CreateDirectory(directoryPath);
        }

        string filePath = _fileSystem.Path.Combine(directoryPath, fileName);
        await _fileSystem.File.WriteAllBytesAsync(filePath, byteContent, cancellationToken);
    }

    public async Task DeleteAsync(string path, CancellationToken cancellationToken)
    {
        if (!_fileSystem.File.Exists(path))
        {
            throw new FileHasNotBeenFoundException();
        }

        _fileSystem.File.Delete(path);
        await Task.CompletedTask;
    }
}