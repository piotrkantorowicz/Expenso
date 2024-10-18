using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Logging;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerLoggingDecorator.Result;

[TestFixture]
internal abstract class
    CommandHandlerLoggingDecoratorTestBase : TestBase<CommandHandlerLoggingDecorator<TestCommand, TestCommandResult>>
{
    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid(),
            Payload: "JYi9R7e7v2Qor");

        _loggerMock = new Mock<ILoggerService<CommandHandlerLoggingDecorator<TestCommand, TestCommandResult>>>();
        _commandHandlerMock = new Mock<ICommandHandler<TestCommand, TestCommandResult>>();
        _serializerMock = new Mock<ISerializer>();

        TestCandidate = new CommandHandlerLoggingDecorator<TestCommand, TestCommandResult>(logger: _loggerMock.Object,
            decorated: _commandHandlerMock.Object, serializer: _serializerMock.Object);
    }

    protected Mock<ICommandHandler<TestCommand, TestCommandResult>> _commandHandlerMock = null!;
    protected Mock<ILoggerService<CommandHandlerLoggingDecorator<TestCommand, TestCommandResult>>> _loggerMock = null!;
    private Mock<ISerializer> _serializerMock = null!;
    protected TestCommand _testCommand = null!;
}