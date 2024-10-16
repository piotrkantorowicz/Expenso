using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;

using FluentAssertions;

using Moq;

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
    public void Should_ThrowFileHasNotBeenFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        const string path = "path";
        _fileSystemMock.Setup(expression: x => x.File.Exists(path)).Returns(value: false);

        // Act
        Func<Task> action = () => TestCandidate.DeleteAsync(path: path, cancellationToken: default);

        // Assert
        action
            .Should()
            .ThrowAsync<FileHasNotBeenFoundException>()
            .WithMessage(expectedWildcardPattern: "One or more validation failures have occurred.")
            .Where(exceptionExpression: ex => ex.Details == "File not found.");
    }
}