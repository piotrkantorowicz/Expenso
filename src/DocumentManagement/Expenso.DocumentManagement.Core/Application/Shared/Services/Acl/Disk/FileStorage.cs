using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

namespace Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk;

internal sealed class FileStorage : IFileStorage
{
    private readonly IFileSystem _fileSystem;

    public FileStorage(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(paramName: nameof(fileSystem));
    }

    public async Task<byte[]> ReadAsync(string path, CancellationToken cancellationToken)
    {
        if (!_fileSystem.File.Exists(path: path))
        {
            throw new FileHasNotBeenFoundException();
        }

        return await _fileSystem.File.ReadAllBytesAsync(path: path, cancellationToken: cancellationToken);
    }

    public async Task SaveAsync(string directoryPath, string fileName, byte[] byteContent,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(value: directoryPath))
        {
            throw new EmptyPathException();
        }

        if (string.IsNullOrEmpty(value: fileName))
        {
            throw new EmptyFileNameException();
        }

        if (byteContent is null || byteContent.Length is 0)
        {
            throw new EmptyFileContentException();
        }

        if (!_fileSystem.Directory.Exists(path: directoryPath))
        {
            _fileSystem.Directory.CreateDirectory(path: directoryPath);
        }

        string filePath = _fileSystem.Path.Combine(path1: directoryPath, path2: fileName);

        await _fileSystem.File.WriteAllBytesAsync(path: filePath, bytes: byteContent,
            cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string path, CancellationToken cancellationToken)
    {
        if (!_fileSystem.File.Exists(path: path))
        {
            throw new FileHasNotBeenFoundException();
        }

        _fileSystem.File.Delete(path: path);
        await Task.CompletedTask;
    }
}