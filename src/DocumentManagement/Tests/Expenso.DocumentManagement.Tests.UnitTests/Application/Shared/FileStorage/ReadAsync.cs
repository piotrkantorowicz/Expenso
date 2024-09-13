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

        _fileSystemMock.Setup(expression: x => x.File.Exists(path)).Returns(value: true);

        _fileSystemMock
            .Setup(expression: x => x.File.ReadAllBytesAsync(path, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: expected);

        // Act
        byte[] result = await TestCandidate.ReadAsync(path: path, cancellationToken: default);

        // Assert
        result.Should().BeEquivalentTo(expectation: expected);
    }

    [Test]
    public void Should_ThrowFileHasNotBeenFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        const string path = "path";
        _fileSystemMock.Setup(expression: x => x.File.Exists(path)).Returns(value: false);

        // Act
        Func<Task> action = () => TestCandidate.ReadAsync(path: path, cancellationToken: default);

        // Assert
        action
            .Should()
            .ThrowAsync<FileHasNotBeenFoundException>()
            .WithMessage(expectedWildcardPattern: "One or more validation failures have occurred")
            .Where(exceptionExpression: x => x.Details == "File not found");
    }
}