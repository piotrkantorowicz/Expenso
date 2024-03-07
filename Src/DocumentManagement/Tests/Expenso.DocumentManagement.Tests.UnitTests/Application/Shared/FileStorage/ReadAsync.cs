using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

using FluentAssertions;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.FileStorage;

internal sealed class ReadAsync : FileStorageTestBase
{
    [Test]
    public async Task Should_ReadFile()
    {
        // Arrange
        const string path = "path";

        byte[] expected =
        [
            1,
            2,
            3
        ];

        _fileSystemMock.Setup(x => x.File.Exists(path)).Returns(true);

        _fileSystemMock
            .Setup(x => x.File.ReadAllBytesAsync(path, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        byte[] result = await TestCandidate.ReadAsync(path, default);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Should_ThrowFileHasNotBeenFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        const string path = "path";
        _fileSystemMock.Setup(x => x.File.Exists(path)).Returns(false);

        // Act
        FileHasNotBeenFoundException? exception =
            Assert.ThrowsAsync<FileHasNotBeenFoundException>(() => TestCandidate.ReadAsync(path, default));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be("One or more validation failures have occurred.");
        exception?.Details.Should().Be("File not found.");
    }
}