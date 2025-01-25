using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.FileStorage;

[TestFixture]
internal sealed class SaveAsync : FileStorageTestBase
{
    [Test]
    public async Task Should_SaveFileIfDirectoryDoesNotExists()
    {
        // Arrange
        const string directoryPath = "directoryPath";
        const string fileName = "fileName";

        byte[] byteContent =
        [
            1,
            2,
            3
        ];

        _fileSystemMock.Setup(expression: x => x.Directory.Exists(directoryPath)).Returns(value: false);
        _fileSystemMock.Setup(expression: x => x.Directory.CreateDirectory(directoryPath));
        _fileSystemMock.Setup(expression: x => x.Path.Combine(directoryPath, fileName)).Returns(value: "filePath");

        _fileSystemMock.Setup(expression: x =>
            x.File.WriteAllBytesAsync("filePath", byteContent, It.IsAny<CancellationToken>()));

        // Act
        await TestCandidate.SaveAsync(directoryPath: directoryPath, fileName: fileName, byteContent: byteContent,
            cancellationToken: default);

        // Assert
        _fileSystemMock.Verify(expression: x => x.Directory.CreateDirectory(directoryPath), times: Times.Once);

        _fileSystemMock.Verify(
            expression: x => x.File.WriteAllBytesAsync("filePath", byteContent, It.IsAny<CancellationToken>()),
            times: Times.Once);
    }

    [Test]
    public async Task Should_SaveFileIfDirectoryExists()
    {
        // Arrange
        const string directoryPath = "directoryPath";
        const string fileName = "fileName";

        byte[] byteContent =
        [
            1,
            2,
            3
        ];

        _fileSystemMock.Setup(expression: x => x.Directory.Exists(directoryPath)).Returns(value: true);
        _fileSystemMock.Setup(expression: x => x.Path.Combine(directoryPath, fileName)).Returns(value: "filePath");

        _fileSystemMock.Setup(expression: x =>
            x.File.WriteAllBytesAsync("filePath", byteContent, It.IsAny<CancellationToken>()));

        // Act
        await TestCandidate.SaveAsync(directoryPath: directoryPath, fileName: fileName, byteContent: byteContent,
            cancellationToken: default);

        // Assert
        _fileSystemMock.Verify(
            expression: x => x.File.WriteAllBytesAsync("filePath", byteContent, It.IsAny<CancellationToken>()),
            times: Times.Once);
    }

    [Test]
    public async Task Should_ThrowEmptyPathException_WhenDirectoryPathIsNullOrWhiteSpace()
    {
        // Arrange
        string directoryPath = string.Empty;
        const string fileName = "fileName";

        byte[] byteContent =
        [
            1,
            2,
            3
        ];

        // Act
        Func<Task> action = () => TestCandidate.SaveAsync(directoryPath: directoryPath, fileName: fileName,
            byteContent: byteContent, cancellationToken: default);

        // Assert
        EmptyPathException? exception = await action.ShouldThrowAsync<EmptyPathException>();
        exception.Message.ShouldBe(expected: "One or more validation failures have occurred.");
        exception.Details.ShouldBe(expected: "Path cannot be empty.");
    }

    [Test]
    public async Task Should_ThrowEmptyFileNameException_WhenFilePathIsNullOrWhiteSpace()
    {
        // Arrange
        const string directoryPath = "directoryPath";
        string fileName = string.Empty;

        byte[] byteContent =
        [
            1,
            2,
            3
        ];

        // Act
        Func<Task> action = () => TestCandidate.SaveAsync(directoryPath: directoryPath, fileName: fileName,
            byteContent: byteContent, cancellationToken: default);

        // Assert
        EmptyFileNameException? exception = await action.ShouldThrowAsync<EmptyFileNameException>();
        exception.Message.ShouldBe(expected: "One or more validation failures have occurred.");
        exception.Details.ShouldBe(expected: "File name cannot be empty.");
    }

    [Test]
    public async Task Should_ThrowFileHasNotBeenFoundException_WhenFileContentIsNull()
    {
        // Arrange
        const string directoryPath = "directoryPath";
        const string fileName = "fileName";
        byte[]? byteContent = null;

        // Act
        Func<Task> action = () => TestCandidate.SaveAsync(directoryPath: directoryPath, fileName: fileName,
            byteContent: byteContent!, cancellationToken: default);

        // Assert
        EmptyFileContentException? exception = await action.ShouldThrowAsync<EmptyFileContentException>();
        exception.Message.ShouldBe(expected: "One or more validation failures have occurred.");
        exception.Details.ShouldBe(expected: "File content cannot be empty.");
    }

    [Test]
    public async Task Should_ThrowEmptyFileContentException_WhenFileContentIsEmpty()
    {
        // Arrange
        const string directoryPath = "directoryPath";
        const string fileName = "fileName";
        byte[] byteContent = [];

        // Act
        Func<Task> action = () => TestCandidate.SaveAsync(directoryPath: directoryPath, fileName: fileName,
            byteContent: byteContent, cancellationToken: default);

        // Assert
        EmptyFileContentException? exception = await action.ShouldThrowAsync<EmptyFileContentException>();
        exception.Message.ShouldBe(expected: "One or more validation failures have occurred.");
        exception.Details.ShouldBe(expected: "File content cannot be empty.");
    }
}