using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.FileStorage;

[TestFixture]
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
        result.ShouldBeEquivalentTo(expected: expected);
    }

    [Test]
    public async Task Should_ThrowFileHasNotBeenFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        const string path = "path";
        _fileSystemMock.Setup(expression: x => x.File.Exists(path)).Returns(value: false);

        // Act
        Func<Task> action = () => TestCandidate.ReadAsync(path: path, cancellationToken: default);

        // Assert
        FileHasNotBeenFoundException? exception = await action.ShouldThrowAsync<FileHasNotBeenFoundException>();
        exception.Message.ShouldBe(expected: "One or more validation failures have occurred.");
        exception.Details.ShouldBe(expected: "File hasn't been found.");
    }
}