using Expenso.Shared.System.Types.Messages;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.Utils.UnitTests;

[TestFixture]
public abstract class TestBase<T> where T : class
{
    [OneTimeSetUp]
    public virtual void OneTimeSetUp()
    {
        MessageContextFactoryMock = new Mock<IMessageContextFactory>();

        MessageContextFactoryMock
            .Setup(expression: x => x.Current(It.IsAny<Guid?>(), It.IsAny<string?>()))
            .Returns(value: new MessageContext(messageId: Guid.NewGuid(), correlationId: Guid.NewGuid(),
                requestedBy: Guid.NewGuid(), timestamp: DateTimeOffset.Now, module: "TestModule"));
    }

    [OneTimeTearDown]
    public virtual void OneTimeTearDown()
    {
        MessageContextFactoryMock = null!;
        TestCandidate = null!;
    }

    protected Mock<IMessageContextFactory> MessageContextFactoryMock { get; set; } = null!;

    protected T TestCandidate { get; set; } = null!;
}