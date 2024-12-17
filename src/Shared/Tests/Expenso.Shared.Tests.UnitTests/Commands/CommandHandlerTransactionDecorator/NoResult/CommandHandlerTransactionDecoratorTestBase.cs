using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Transactions;
using Expenso.Shared.Database;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerTransactionDecorator.NoResult;

[TestFixture]
internal abstract class
    CommandHandlerTransactionDecoratorTestBase : TestBase<CommandHandlerTransactionDecorator<TestCommand>>
{
    [SetUp]
    public void Setup()
    {
        _testCommand = new TestCommand(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid(),
            Payload: "JYi9R7e7v2Qor");

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _commandHandlerMock = new Mock<ICommandHandler<TestCommand>>();

        TestCandidate = new CommandHandlerTransactionDecorator<TestCommand>(unitOfWork: _unitOfWorkMock.Object,
            decorated: _commandHandlerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
        _commandHandlerMock.Reset();
        _testCommand = null!;
        _unitOfWorkMock = null!;
        _commandHandlerMock = null!;
        TestCandidate = null!;
    }

    protected Mock<ICommandHandler<TestCommand>> _commandHandlerMock = null!;
    protected TestCommand _testCommand = null!;
    protected Mock<IUnitOfWork> _unitOfWorkMock = null!;
}