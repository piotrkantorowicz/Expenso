using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;
using Expenso.Shared.Tests.Utils.UnitTests;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandler.Result;

[TestFixture]
internal abstract class CommandHandlerResultTestBase : TestBase<TestCommandHandler>
{
    [SetUp]
    public void Setup()
    {
        _testCommand = new TestCommand(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid(),
            Payload: "TkpxYGL8bVkwqDIo");

        TestCandidate = new TestCommandHandler();
    }

    [TearDown]
    public void TearDown()
    {
        TestCandidate = null!;
        _testCommand = null!;
    }

    protected TestCommand _testCommand = null!;
}