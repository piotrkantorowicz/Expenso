using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Validations;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerDecorator.NoResult;

internal abstract class CommandHandlerDecoratorTestBase : TestBase<CommandHandlerValidationDecorator<TestCommand>>
{
    protected TestCommand _testCommand = null!;
    protected Mock<ICommandValidator<TestCommand>> _validator = null!;

    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(Guid.NewGuid(), "JYi9R7e7v2Qor");
        _validator = new Mock<ICommandValidator<TestCommand>>();
        Mock<ICommandHandler<TestCommand>> handler = new();

        TestCandidate = new CommandHandlerValidationDecorator<TestCommand>([_validator.Object],
                handler.Object);
    }
}
