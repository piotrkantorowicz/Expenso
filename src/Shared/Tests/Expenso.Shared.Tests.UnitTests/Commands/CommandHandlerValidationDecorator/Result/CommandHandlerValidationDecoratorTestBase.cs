using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerValidationDecorator.Result;

[TestFixture]
internal abstract class
    CommandHandlerValidationDecoratorTestBase : TestBase<
    CommandHandlerValidationDecorator<TestCommand, TestCommandResult>>
{
    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid(),
            Name: "JYi9R7e7v2Qor");

        _validator = new Mock<ICommandValidator<TestCommand>>();
        _handler = new Mock<ICommandHandler<TestCommand, TestCommandResult>>();

        TestCandidate =
            new CommandHandlerValidationDecorator<TestCommand, TestCommandResult>(validators: [_validator.Object],
                decorated: _handler.Object);
    }

    protected Mock<ICommandHandler<TestCommand, TestCommandResult>> _handler = null!;
    protected TestCommand _testCommand = null!;
    protected Mock<ICommandValidator<TestCommand>> _validator = null!;
}