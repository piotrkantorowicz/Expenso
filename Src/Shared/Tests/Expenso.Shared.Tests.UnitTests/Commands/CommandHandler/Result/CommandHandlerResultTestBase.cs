using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandler.Result;

internal abstract class CommandHandlerResultTestBase : TestBase<TestCommandHandler>
{
    protected TestCommand _testCommand = null!;

    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(Guid.NewGuid(), "TkpxYGL8bVkwqDIo");
        TestCandidate = new TestCommandHandler();
    }
}