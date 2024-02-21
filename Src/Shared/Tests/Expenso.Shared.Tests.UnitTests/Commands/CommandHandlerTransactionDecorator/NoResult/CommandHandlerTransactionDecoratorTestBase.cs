using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Transactions;
using Expenso.Shared.Database;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerTransactionDecorator.NoResult;

internal abstract class
    CommandHandlerTransactionDecoratorTestBase : TestBase<CommandHandlerTransactionDecorator<TestCommand>>
{
    protected TestCommand _testCommand = null!;
    protected Mock<ICommandHandler<TestCommand>> _commandHandlerMock = null!;
    protected Mock<IUnitOfWork> _unitOfWorkMock = null!;

    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(MessageContextFactoryMock.Object.Current(), Guid.NewGuid(), "JYi9R7e7v2Qor");
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _commandHandlerMock = new Mock<ICommandHandler<TestCommand>>();

        TestCandidate =
            new CommandHandlerTransactionDecorator<TestCommand>(_unitOfWorkMock.Object, _commandHandlerMock.Object);
    }
}