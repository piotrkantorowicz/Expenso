using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandler.NoResult;

[TestFixture]
internal abstract class CommandHandlerNoResultTestBase : TestBase<TestCommandHandler>
{
    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid(),
            Payload: "laFrGWWfwLzmq");

        _loggerMock = new Mock<ILoggerService<TestCommandHandler>>();
        TestCandidate = new TestCommandHandler(logger: _loggerMock.Object);
    }

    protected Mock<ILoggerService<TestCommandHandler>> _loggerMock = null!;
    protected TestCommand _testCommand = null!;
}