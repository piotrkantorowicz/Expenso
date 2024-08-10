using Expenso.Shared.System.Types.Messages;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Moq;

namespace Expenso.Shared.Tests.Utils.UnitTests;

public abstract class TestBase<T> where T : class
{
    private readonly MessageContext _messageContext = new(messageId: Guid.NewGuid(), correlationId: Guid.NewGuid(),
        requestedBy: Guid.NewGuid(), timestamp: DateTimeOffset.Now);

    protected Mock<IMessageContextFactory> MessageContextFactoryMock { get; set; } = null!;

    protected T TestCandidate { get; set; } = null!;

    [OneTimeSetUp]
    public virtual void OneTimeSetUp()
    {
        MessageContextFactoryMock = new Mock<IMessageContextFactory>();
        MessageContextFactoryMock.Setup(expression: x => x.Current(It.IsAny<Guid?>())).Returns(value: _messageContext);
    }
}