using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Validations;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerDecorators.NoResult;

internal abstract class
    CommandHandlerDecoratorTests : TestBase<Shared.Commands.Decorators.CommandHandlerValidationDecorator<TestCommand>>
{
    protected TestCommand _testCommand = null!;
    protected Mock<ICommandValidator<TestCommand>> _validator = null!;

    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(Guid.NewGuid(), "JYi9R7e7v2Qor");
        _validator = new Mock<ICommandValidator<TestCommand>>();
        Mock<ICommandHandler<TestCommand>> handler = new();

        TestCandidate =
            new Shared.Commands.Decorators.CommandHandlerValidationDecorator<TestCommand>([_validator.Object],
                handler.Object);
    }
}

internal sealed record TestCommand(Guid Id, string Name) : ICommand;