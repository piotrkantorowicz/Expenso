using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Logging;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerLoggingDecorator.NoResult;

internal abstract class CommandHandlerLoggingDecoratorTestBase : TestBase<CommandHandlerLoggingDecorator<TestCommand>>
{
    protected Mock<ICommandHandler<TestCommand>> _commandHandlerMock = null!;
    protected Mock<ILogger<CommandHandlerLoggingDecorator<TestCommand>>> _loggerMock = null!;
    private Mock<ISerializer> _serializerMock = null!;
    protected TestCommand _testCommand = null!;

    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(MessageContextFactoryMock.Object.Current(), Guid.NewGuid(), "JYi9R7e7v2Qor");
        _loggerMock = new Mock<ILogger<CommandHandlerLoggingDecorator<TestCommand>>>();
        _commandHandlerMock = new Mock<ICommandHandler<TestCommand>>();
        _serializerMock = new Mock<ISerializer>();

        TestCandidate = new CommandHandlerLoggingDecorator<TestCommand>(_loggerMock.Object, _commandHandlerMock.Object,
            _serializerMock.Object);
    }
}