using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

using FluentValidation;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerFluentValidationDecorator.NoResult;

internal abstract class
    CommandHandlerFluentValidationDecoratorTestBase : TestBase<CommandHandlerFluentValidationDecorator<TestCommand>>
{
    protected TestCommand _testCommand = null!;
    protected Mock<IValidator<TestCommand>> _validator = null!;

    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid(),
            Name: "JYi9R7e7v2Qor");

        _validator = new Mock<IValidator<TestCommand>>();
        Mock<ICommandHandler<TestCommand>> handler = new();

        TestCandidate =
            new CommandHandlerFluentValidationDecorator<TestCommand>(validators: [_validator.Object],
                decorated: handler.Object);
    }
}