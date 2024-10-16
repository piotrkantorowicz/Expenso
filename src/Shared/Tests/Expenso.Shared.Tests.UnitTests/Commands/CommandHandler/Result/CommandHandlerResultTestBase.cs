using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandler.Result;

[TestFixture]
internal abstract class CommandHandlerResultTestBase : TestBase<TestCommandHandler>
{
    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid(),
            Name: "TkpxYGL8bVkwqDIo");

        TestCandidate = new TestCommandHandler();
    }

    protected TestCommand _testCommand = null!;
}