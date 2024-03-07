using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

using FluentAssertions;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.FileStorage;

internal sealed class DeleteAsync : FileStorageTestBase
{
    [Test]
    public async Task Should_DeleteFile()
    {
        // Arrange
        const string path = "path";
        _fileSystemMock.Setup(x => x.File.Exists(path)).Returns(true);
        _fileSystemMock.Setup(x => x.File.Delete(path));

        // Act
        await TestCandidate.DeleteAsync(path, default);

        // Assert
        _fileSystemMock.Verify(x => x.File.Delete(path), Times.Once);
    }

    [Test]
    public void Should_ThrowFileHasNotBeenFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        const string path = "path";
        _fileSystemMock.Setup(x => x.File.Exists(path)).Returns(false);

        // Act
        FileHasNotBeenFoundException? exception =
            Assert.ThrowsAsync<FileHasNotBeenFoundException>(() => TestCandidate.DeleteAsync(path, default));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be("One or more validation failures have occurred.");
        exception?.Details.Should().Be("File not found.");
    }
}