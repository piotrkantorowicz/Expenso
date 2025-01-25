using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.FileStorage;

[TestFixture]
internal sealed class DeleteAsync : FileStorageTestBase
{
    [Test]
    public async Task Should_DeleteFile()
    {
        // Arrange
        const string path = "path";
        _fileSystemMock.Setup(expression: x => x.File.Exists(path)).Returns(value: true);
        _fileSystemMock.Setup(expression: x => x.File.Delete(path));

        // Act
        await TestCandidate.DeleteAsync(path: path, cancellationToken: default);

        // Assert
        _fileSystemMock.Verify(expression: x => x.File.Delete(path), times: Times.Once);
    }

    [Test]
    public async Task Should_ThrowFileHasNotBeenFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        const string path = "path";
        _fileSystemMock.Setup(expression: x => x.File.Exists(path)).Returns(value: false);

        // Act
        Func<Task> action = () => TestCandidate.DeleteAsync(path: path, cancellationToken: default);

        // Assert
        FileHasNotBeenFoundException? exception = await action.ShouldThrowAsync<FileHasNotBeenFoundException>();
        exception.Message.ShouldBe(expected: "One or more validation failures have occurred.");
        exception.Details.ShouldBe(expected: "File hasn't been found.");
    }
}