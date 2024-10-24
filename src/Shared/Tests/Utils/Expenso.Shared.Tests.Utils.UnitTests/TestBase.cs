﻿using Expenso.Shared.System.Types.Messages;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Moq;

namespace Expenso.Shared.Tests.Utils.UnitTests;

[TestFixture]
public abstract class TestBase<T> where T : class
{
    [OneTimeSetUp]
    public virtual void OneTimeSetUp()
    {
        MessageContextFactoryMock = new Mock<IMessageContextFactory>();
        MessageContextFactoryMock.Setup(expression: x => x.Current(It.IsAny<Guid?>())).Returns(value: _messageContext);
    }

    [OneTimeTearDown]
    public virtual void OneTimeTearDown()
    {
        MessageContextFactoryMock = null!;
        TestCandidate = null!;
    }

    private readonly MessageContext _messageContext = new(messageId: Guid.NewGuid(), correlationId: Guid.NewGuid(),
        requestedBy: Guid.NewGuid(), timestamp: DateTimeOffset.Now, module: "TestModule");

    protected Mock<IMessageContextFactory> MessageContextFactoryMock { get; set; } = null!;

    protected T TestCandidate { get; set; } = null!;
}