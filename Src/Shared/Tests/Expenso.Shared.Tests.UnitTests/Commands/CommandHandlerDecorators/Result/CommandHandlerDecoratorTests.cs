using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Validations;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerDecorators.Result;

internal abstract class
    CommandHandlerDecoratorTests : TestBase<CommandHandlerValidationDecorator<TestCommand, TestCommandResult>>
{
    protected Mock<ICommandHandler<TestCommand, TestCommandResult>> _handler = null!;
    protected TestCommand _testCommand = null!;
    protected Mock<ICommandValidator<TestCommand>> _validator = null!;

    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(Guid.NewGuid(), "JYi9R7e7v2Qor");
        _validator = new Mock<ICommandValidator<TestCommand>>();
        _handler = new Mock<ICommandHandler<TestCommand, TestCommandResult>>();

        TestCandidate =
            new CommandHandlerValidationDecorator<TestCommand, TestCommandResult>([_validator.Object], _handler.Object);
    }
}