using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandler.NoResult;

internal abstract class CommandHandlerNoResultTestBase : TestBase<TestCommandHandler>
{
    protected Mock<ILoggerService<TestCommandHandler>> _loggerMock = null!;
    protected TestCommand _testCommand = null!;

    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid(),
            Name: "laFrGWWfwLzmq");

        _loggerMock = new Mock<ILoggerService<TestCommandHandler>>();
        TestCandidate = new TestCommandHandler(logger: _loggerMock.Object);
    }
}