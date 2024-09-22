using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerTransactionDecorator.Result;

internal sealed class HandleAsync : CommandHandlerTransactionDecoratorTestBase
{
    [Test]
    public async Task Should_BeginAndCommitTransaction_When_NoExceptionOccurred()
    {
        // Arrange
        // Act
        await TestCandidate.HandleAsync(command: _testCommand, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _unitOfWorkMock.Verify(expression: x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()),
            times: Times.Once);

        _unitOfWorkMock.Verify(
            expression: x => x.CommitTransactionAsync(It.IsAny<CancellationToken>(), It.IsAny<bool>()),
            times: Times.Once);
    }

    [Test]
    public async Task Should_BeginAndRollback_When_NoExceptionOccurred()
    {
        // Arrange
        _commandHandlerMock
            .Setup(expression: x => x.HandleAsync(_testCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new Exception(message: "Intentional thrown to test rollback."));

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(command: _testCommand, cancellationToken: CancellationToken.None);

        // Assert
        await action
            .Should()
            .ThrowAsync<Exception>()
            .WithMessage(expectedWildcardPattern: "Intentional thrown to test rollback.");

        _unitOfWorkMock.Verify(expression: x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()),
            times: Times.Once);

        _unitOfWorkMock.Verify(expression: x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()),
            times: Times.Once);
    }
}