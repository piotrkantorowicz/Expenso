using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerTransactionDecorator.NoResult;

internal sealed class HandleAsync : CommandHandlerTransactionDecoratorTestBase
{
    [Test]
    public async Task Should_BeginAndCommitTransaction_When_NoExceptionOccurred()
    {
        // Arrange
        // Act
        await TestCandidate.HandleAsync(_testCommand, It.IsAny<CancellationToken>());

        // Assert
        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>(), It.IsAny<bool>()),
            Times.Once);
    }

    [Test]
    public void Should_BeginAndRollback_When_NoExceptionOccurred()
    {
        // Arrange
        _commandHandlerMock
            .Setup(x => x.HandleAsync(_testCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Intentional thrown to test rollback"));

        // Act
        Exception? exception =
            Assert.ThrowsAsync<Exception>(() => TestCandidate.HandleAsync(_testCommand, CancellationToken.None));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be("Intentional thrown to test rollback");
        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}