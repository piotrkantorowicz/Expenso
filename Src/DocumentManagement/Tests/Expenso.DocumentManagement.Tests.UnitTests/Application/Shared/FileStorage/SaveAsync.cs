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

        _fileSystemMock.Setup(x => x.Directory.Exists(directoryPath)).Returns(false);
        _fileSystemMock.Setup(x => x.Directory.CreateDirectory(directoryPath));
        _fileSystemMock.Setup(x => x.Path.Combine(directoryPath, fileName)).Returns("filePath");
        _fileSystemMock.Setup(x => x.File.WriteAllBytesAsync("filePath", byteContent, It.IsAny<CancellationToken>()));

        // Act
        await TestCandidate.SaveAsync(directoryPath, fileName, byteContent, default);

        // Assert
        _fileSystemMock.Verify(x => x.Directory.CreateDirectory(directoryPath), Times.Once);

        _fileSystemMock.Verify(x => x.File.WriteAllBytesAsync("filePath", byteContent, It.IsAny<CancellationToken>()),
            Times.Once);
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

        _fileSystemMock.Setup(x => x.Directory.Exists(directoryPath)).Returns(true);
        _fileSystemMock.Setup(x => x.Path.Combine(directoryPath, fileName)).Returns("filePath");
        _fileSystemMock.Setup(x => x.File.WriteAllBytesAsync("filePath", byteContent, It.IsAny<CancellationToken>()));

        // Act
        await TestCandidate.SaveAsync(directoryPath, fileName, byteContent, default);

        // Assert
        _fileSystemMock.Verify(x => x.File.WriteAllBytesAsync("filePath", byteContent, It.IsAny<CancellationToken>()),
            Times.Once);
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
        EmptyPathException? exception = Assert.ThrowsAsync<EmptyPathException>(() =>
            TestCandidate.SaveAsync(directoryPath, fileName, byteContent, default));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be("One or more validation failures have occurred.");
        exception?.Details.Should().Be("Path cannot be empty.");
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
        EmptyFileNameException? exception = Assert.ThrowsAsync<EmptyFileNameException>(() =>
            TestCandidate.SaveAsync(directoryPath, fileName, byteContent, default));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be("One or more validation failures have occurred.");
        exception?.Details.Should().Be("File name cannot be empty.");
    }

    [Test]
    public void Should_ThrowFileHasNotBeenFoundException_WhenFileContentIsNull()
    {
        // Arrange
        const string directoryPath = "directoryPath";
        const string fileName = "fileName";
        byte[]? byteContent = null;

        // Act
        EmptyFileContentException? exception = Assert.ThrowsAsync<EmptyFileContentException>(() =>
            TestCandidate.SaveAsync(directoryPath, fileName, byteContent!, default));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be("One or more validation failures have occurred.");
        exception?.Details.Should().Be("File content cannot be empty.");
    }

    [Test]
    public void Should_ThrowFileHasNotBeenFoundException_WhenFileContentIsEmpty()
    {
        // Arrange
        const string directoryPath = "directoryPath";
        const string fileName = "fileName";
        byte[] byteContent = Array.Empty<byte>();

        // Act
        EmptyFileContentException? exception = Assert.ThrowsAsync<EmptyFileContentException>(() =>
            TestCandidate.SaveAsync(directoryPath, fileName, byteContent, default));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be("One or more validation failures have occurred.");
        exception?.Details.Should().Be("File content cannot be empty.");
    }
}