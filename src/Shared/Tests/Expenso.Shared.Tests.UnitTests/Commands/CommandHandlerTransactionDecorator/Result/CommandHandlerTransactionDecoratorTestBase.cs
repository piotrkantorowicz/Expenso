using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Transactions;
using Expenso.Shared.Database;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerTransactionDecorator.Result;

[TestFixture]
internal abstract class
    CommandHandlerTransactionDecoratorTestBase : TestBase<
    CommandHandlerTransactionDecorator<TestCommand, TestCommandResult>>
{
    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid(),
            Payload: "JYi9R7e7v2Qor");

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _commandHandlerMock = new Mock<ICommandHandler<TestCommand, TestCommandResult>>();

        TestCandidate =
            new CommandHandlerTransactionDecorator<TestCommand, TestCommandResult>(unitOfWork: _unitOfWorkMock.Object,
                decorated: _commandHandlerMock.Object);
    }

    protected Mock<ICommandHandler<TestCommand, TestCommandResult>> _commandHandlerMock = null!;
    protected TestCommand _testCommand = null!;
    protected Mock<IUnitOfWork> _unitOfWorkMock = null!;
}