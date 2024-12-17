using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandler.NoResult;

[TestFixture]
internal abstract class CommandHandlerNoResultTestBase : TestBase<TestCommandHandler>
{
    [SetUp]
    public void Setup()
    {
        _testCommand = new TestCommand(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid(),
            Payload: "laFrGWWfwLzmq");

        _loggerMock = new Mock<ILoggerService<TestCommandHandler>>();
        TestCandidate = new TestCommandHandler(logger: _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _loggerMock.Reset();
        _testCommand = null!;
        TestCandidate = null!;
        _loggerMock = null!;
    }

    protected Mock<ILoggerService<TestCommandHandler>> _loggerMock = null!;
    protected TestCommand _testCommand = null!;
}