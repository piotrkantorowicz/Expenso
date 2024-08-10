using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

using FluentAssertions;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.FileStorage;

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
    public void Should_ThrowFileHasNotBeenFoundException_WhenDirectoryPathIsNullOrEmpty()
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
        EmptyPathException? exception = Assert.ThrowsAsync<EmptyPathException>(code: () =>
            TestCandidate.SaveAsync(directoryPath: directoryPath, fileName: fileName, byteContent: byteContent,
                cancellationToken: default));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be(expected: "One or more validation failures have occurred.");
        exception?.Details.Should().Be(expected: "Path cannot be empty.");
    }

    [Test]
    public void Should_ThrowFileHasNotBeenFoundException_WhenFilePathIsNullOrEmpty()
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
        EmptyFileNameException? exception = Assert.ThrowsAsync<EmptyFileNameException>(code: () =>
            TestCandidate.SaveAsync(directoryPath: directoryPath, fileName: fileName, byteContent: byteContent,
                cancellationToken: default));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be(expected: "One or more validation failures have occurred.");
        exception?.Details.Should().Be(expected: "File name cannot be empty.");
    }

    [Test]
    public void Should_ThrowFileHasNotBeenFoundException_WhenFileContentIsNull()
    {
        // Arrange
        const string directoryPath = "directoryPath";
        const string fileName = "fileName";
        byte[]? byteContent = null;

        // Act
        EmptyFileContentException? exception = Assert.ThrowsAsync<EmptyFileContentException>(code: () =>
            TestCandidate.SaveAsync(directoryPath: directoryPath, fileName: fileName, byteContent: byteContent!,
                cancellationToken: default));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be(expected: "One or more validation failures have occurred.");
        exception?.Details.Should().Be(expected: "File content cannot be empty.");
    }

    [Test]
    public void Should_ThrowFileHasNotBeenFoundException_WhenFileContentIsEmpty()
    {
        // Arrange
        const string directoryPath = "directoryPath";
        const string fileName = "fileName";
        byte[] byteContent = Array.Empty<byte>();

        // Act
        EmptyFileContentException? exception = Assert.ThrowsAsync<EmptyFileContentException>(code: () =>
            TestCandidate.SaveAsync(directoryPath: directoryPath, fileName: fileName, byteContent: byteContent,
                cancellationToken: default));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be(expected: "One or more validation failures have occurred.");
        exception?.Details.Should().Be(expected: "File content cannot be empty.");
    }
}