using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandler.NoResult;

internal abstract class CommandHandlerNoResultTestBase : TestBase<TestCommandHandler>
{
    protected Mock<ILogger<TestCommandHandler>> _loggerMock = null!;
    protected TestCommand _testCommand = null!;

    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid(),
            Name: "laFrGWWfwLzmq");

        _loggerMock = new Mock<ILogger<TestCommandHandler>>();
        TestCandidate = new TestCommandHandler(logger: _loggerMock.Object);
    }
}